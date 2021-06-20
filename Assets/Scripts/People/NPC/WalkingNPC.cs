using System;
using System.Collections;
using People.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace People.NPC
{
    public enum PathPending
    {
        None,
        Found,
        Searching
    }
    public class WalkingNPC : NPCdata
    {
        private Animator _anim;
        private NavMeshAgent _agent;

        private GameObject _iaPoints;

        private Transform[] _positions;

        public bool RandomPosition = false;
        private int _pos = 0;
        private bool _shoved = false;

        private Vector3 _nextPoint;
        private PathPending _hasFoundNextDestination = PathPending.None;

        public Vector3 parentPosition = Vector3.up;
        public float searchRadius = 15;
        public bool findInSphere;
        public GameObject EventZone { get; private set; }

        public void SetEventZone(GameObject eventZone) => EventZone = eventZone;

        void Awake()
        {
            // positions = iaPoints.GetComponentsInChildren<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            // agent.autoBraking = false;
            _anim = GetComponent<Animator>();
            // SetStatus(NpcStatus.Walking);
            if (Status == NpcStatus.Walking)
            {
                _anim.SetBool("walk", true);
            }
        }

        public void SetIAPoints(Transform[] points) => _positions = points;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerControler>().Running() &&
                !_shoved)
            {
                _agent.isStopped = true;
                _shoved = true;
                Vector3 pos = GetComponent<CapsuleCollider>().ClosestPointOnBounds(other.gameObject.transform.position);
                bool right = pos.normalized.x < 0;
                bool forward = pos.normalized.z < 0;
                // transform.position -= Vector3.right * pos.normalized.x * .4f;

                _anim.SetBool("mirror", right);
                _anim.Play("shove");
                StartCoroutine(WaitCoroutine());
            }
        }

        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(3);
            _agent.isStopped = false;
            _shoved = false;
        }

        // Update is called once per frame
        private void Start()
        {
            _agent.destination = transform.position;
        }

        void Update()
        {
            if (_agent.remainingDistance < 5 && _hasFoundNextDestination == PathPending.None)
                FindNextPoint();
            if (_agent.remainingDistance < 0.5f && _hasFoundNextDestination == PathPending.Found)
            {
                _agent.destination = _nextPoint;
                _hasFoundNextDestination = PathPending.None;
            }

            _agent.speed = Physics.Raycast(transform.position + Vector3.up, transform.forward, 1f, 768) ? 1.2f : 2;
        }

        private void FindNextPoint()
        {
            if (RandomPosition)
                FindRandomDestination(searchRadius);
            else
            {
                _pos = (_pos + 1) % _positions.Length;
                _nextPoint = _positions[_pos].position;
            }
        }

        public void FindRandomDestination(float range = 0)
        {
            _hasFoundNextDestination = PathPending.Searching;
            if (range == 0)
                range = searchRadius;
            var navMeshPath = new NavMeshPath();
            Vector3 newPos;
            do
            {
                newPos = _agent.destination + Random.insideUnitSphere * range;
                if (!findInSphere)
                    newPos.y = parentPosition.y;
            } while (NavMesh.CalculatePath(_agent.destination, newPos, NavMesh.AllAreas, navMeshPath) &&
                     navMeshPath.status != NavMeshPathStatus.PathComplete);

            _hasFoundNextDestination = PathPending.Found;
            _nextPoint = newPos;
        }
    }
}