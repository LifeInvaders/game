using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ExitGames.Client.Photon.StructWrapping;
using People.Player;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Pun.Simple;
using UnityEngine.Experimental.AI;
using Utilities;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;


namespace People.NPC
{
    public class PhotonNPC : MonoBehaviourPunCallbacks
    {
        public bool start;
        private Animator _anim;
        private NavMeshAgent _agent;
        private Queue<Vector3> points;
        private Vector3 lastDest;
        private int activationTime;
        private double internalClock;
        private GameObject EventZone;
        public bool inZone;

        void Start()
        {
            points = new Queue<Vector3>();
            Random r = new Random(gameObject.GetPhotonView().ViewID);
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            if (!_agent.isOnNavMesh)
            {
                NavMesh.SamplePosition(transform.position,out var hit, 10.0f, NavMesh.AllAreas);
                _agent.Warp(hit.position);
            }
            _agent.radius = 0.1f;
            _agent.speed = 2;
            StartCoroutine(BootUp(r.Next(25,31)));
        }

        [PunRPC]
        void SetClock(double activation)
        {
            internalClock = activation;
        }

        public void SetEventZone(GameObject go) => EventZone = go;

        public GameObject GetEventZone() => EventZone;
        

        public IEnumerator BootUp(double time)
        {
            _agent.ResetPath();
            start = false;
            internalClock = 0;
            lastDest = Vector3.zero;
            points.Clear();
            points.Enqueue(Vector3.zero);
            if (PhotonNetwork.IsMasterClient)
            {
                gameObject.GetPhotonView().RPC(nameof(SetClock),RpcTarget.All,PhotonNetwork.Time + time);
                CalculateNextPath(transform.position);
            } 
            yield return new WaitUntil(CheckNpcSyncTime);
            if (PhotonNetwork.IsMasterClient)
                yield return new WaitForSeconds(2); 
            GotoNextPoint();
            start = true;
            _anim.SetBool("walk", true);
        }

        public void ResetNpc()
        {
            _anim.SetBool("walk", false);
            _agent.ResetPath();
            start = false;
            points.Clear();
        }

        bool CheckNpcSyncTime() => internalClock != 0 && PhotonNetwork.Time >= Convert.ToDouble(internalClock);

        void FixedUpdate()
        {
            if (!start) return;
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            {
                Debug.Log("Going towards next point...");
                GotoNextPoint();
            }
            //_agent.speed = Physics.Raycast(transform.position + Vector3.up, transform.forward, 1f, 768) ? 1.2f : 2;
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