using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using People.Player;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using UnityEngine.Experimental.AI;
using Vector3 = UnityEngine.Vector3;


namespace People.NPC
{
    public class PhotonNPC1 : NPCdata
    {
        private bool _start;
        private Animator _anim;
        private NavMeshAgent _agent;
        private System.Random _random;
        private Queue<Vector3> points;
        private Vector3 lastDest;
        
        
        void Start()
        {
            _random = new System.Random();
                // positions = iaPoints.GetComponentsInChildren<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            // agent.autoBraking = false;
            _anim = GetComponent<Animator>();
            // SetStatus(NpcStatus.Walking);
            if (Status == NpcStatus.Walking)
                _anim.SetBool("walk", true);
            points = new Queue<Vector3>();
            points.Enqueue(Vector3.zero);
            lastDest = Vector3.zero;
            if (!_agent.isOnNavMesh)
            {
                NavMesh.SamplePosition(transform.position,out var hit, 10.0f, NavMesh.AllAreas);
                _agent.Warp(hit.position);
            }
            StartCoroutine(StartUpdate());
            if (PhotonNetwork.IsMasterClient) CalculateNextPath(transform.position);
        }

        IEnumerator StartUpdate()
        {
            Debug.Log("Waiting 10 seconds...");
            yield return new WaitForSeconds(20);
            Debug.Log("Script started!");
            GotoNextPoint();
            _start = true;
        }
        
        void FixedUpdate()
        {
            if (!_start) return;
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                Debug.Log("Going towards next point...");
                GotoNextPoint();
            }
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, 1f, 768))
                _agent.speed = 1.2f;
            else
                _agent.speed = 2;
        }
        
        private void GotoNextPoint()
        {
            Vector3 dest = points.Dequeue();
            if (dest == Vector3.zero)
            {
                dest = points.Dequeue();
                if (PhotonNetwork.IsMasterClient)
                    CalculateNextPath(lastDest);
            }
            _agent.SetDestination(dest);
        }


        private void CalculateNextPath(Vector3 start)
        {
            int tryPaths = 0;
            int tryPositions = 0;
            bool validPath;
            NavMeshPath path = new NavMeshPath();
            do
            {
                bool validPosition;
                NavMeshHit hit;
                do
                {
                    Vector3 randomDirection = transform.forward * 5 + start + UnityEngine.Random.insideUnitSphere * 15;
                    if (tryPaths > 500) randomDirection.y = start.y;
                    validPosition = NavMesh.SamplePosition(randomDirection, out hit, 20, NavMesh.AllAreas);
                } while (!validPosition && ++tryPositions < 1000 && path.status != NavMeshPathStatus.PathComplete);
                validPath = NavMesh.CalculatePath(start, hit.position, NavMesh.AllAreas, path);
            } while (!validPath && ++tryPaths < 1000);
            gameObject.GetPhotonView().RPC(nameof(SetNextPath), RpcTarget.All, path.corners);
            }

        [PunRPC]
        public void SetNextPath(Vector3[] vectArray)
        {
            Debug.Log("adding vectors to queue");
            foreach (Vector3 vect in vectArray)
            {
                Debug.Log("Adding vector...");
                points.Enqueue(vect);
                
            }
            Debug.Log("Done adding vectors.");
            points.Enqueue(Vector3.zero);
            lastDest = vectArray.Last();
        }
    }
}