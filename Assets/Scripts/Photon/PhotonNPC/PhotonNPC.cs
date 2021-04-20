using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Vector3 = UnityEngine.Vector3;


namespace People.NPC
{
    public class PhotonNPC : NPCdata
    {
        private bool _start;
        private Animator _anim;
        private NavMeshAgent _agent;
        private NavMeshPath _path;


        void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                enabled = false;
                return;
            }
            // positions = iaPoints.GetComponentsInChildren<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            // agent.autoBraking = false;
            _anim = GetComponent<Animator>();
            // SetStatus(NpcStatus.Walking);
            if (Status == NpcStatus.Walking)
                _anim.SetBool("walk", true);
            if (!_agent.isOnNavMesh)
            {
                NavMesh.SamplePosition(transform.position,out var hit, 10.0f, NavMesh.AllAreas);
                _agent.Warp(hit.position);
            }
            CalculateNextPath(transform.position);
        }
        
        
        void FixedUpdate()
        {
            if (PhotonNetwork.IsMasterClient)
            if (_start) return;
            if (!_agent.hasPath)
            {
                Debug.Log("Going towards next point...");
                ConsumePath();
            }
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, 1f, 768))
                _agent.speed = 1.2f;
            else
                _agent.speed = 2;
        }
        
        private void ConsumePath()
        {
            _agent.SetPath(_path);
            CalculateNextPath(_path.corners.Last());
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
                    Vector3 randomDirection = transform.forward * 5 + start + Random.insideUnitSphere * 15;
                    if (tryPaths > 500) randomDirection.y = start.y;
                    validPosition = NavMesh.SamplePosition(randomDirection, out hit, 20, NavMesh.AllAreas);
                } while (!validPosition && ++tryPositions < 1000 && path.status != NavMeshPathStatus.PathComplete);
                validPath = NavMesh.CalculatePath(start, hit.position, NavMesh.AllAreas, path);
            } while (!validPath && ++tryPaths < 1000);
            _path = path;
            if (!_start) _start = true;
        }
    }
}