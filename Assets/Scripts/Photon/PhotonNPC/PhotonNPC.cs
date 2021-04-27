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
using Utilities;
using Vector3 = UnityEngine.Vector3;


namespace People.NPC
{
    public class PhotonNPC : NPCdata
    {
        private bool _start;
        private Animator _anim;
        private NavMeshAgent _agent;
        private Queue<Vector3> points;
        private Vector3 lastDest;

        void Start()
        {
            StartCoroutine(WaitSync());
            // positions = iaPoints.GetComponentsInChildren<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
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
            _agent.radius = 0.1f;
            if (PhotonNetwork.IsMasterClient)
                CalculateNextPath(transform.position);
        }

        IEnumerator WaitSync()
        {
            yield return new WaitUntil(CheckNpcSyncTime);
            if (PhotonNetwork.IsMasterClient)
                yield return new WaitForSeconds(2);
            GotoNextPoint();
            _start = true;
        }

        bool CheckNpcSyncTime()
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("syncNpcStart", out var time))
                return false;
            return PhotonNetwork.Time >= Convert.ToDouble(time);
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
            if (!validPath)
            {
                Debug.Log("Failed to find a path! Destroying NPC...");
                PhotonNetwork.Destroy(gameObject);
                return;
            }
            gameObject.GetPhotonView().RPC(nameof(SetNextPath),RpcTarget.All,path.corners);
        }

        [PunRPC]
        public void SetNextPath(Vector3[] vectArray)
        {
            foreach (Vector3 vect in vectArray)
                points.Enqueue(vect);
            points.Enqueue(Vector3.zero);
            lastDest = vectArray.Last();
        }
    }
}