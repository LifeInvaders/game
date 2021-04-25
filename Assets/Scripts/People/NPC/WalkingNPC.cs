using System.Collections;
using People.Player;
using UnityEngine;
using UnityEngine.AI;

namespace People.NPC
{
    public class WalkingNPC : NPCdata
    {
        private Animator _anim;
        private NavMeshAgent _agent;

        private GameObject _iaPoints;

        public Transform[] positions;

        public bool RandomPosition = false;
        private int _pos = 0;
        private bool _shoved = false;

        private Vector3 _nextpoint;

        public Vector3 ParentPosition = Vector3.up;
        public float SearchRadius = 15;
        public bool FindInSphere;

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

        public void SetIAPoints(Transform[] points) => positions = points;

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
        void Update()
        {
            if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
                GotoNextPoint();
            // Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.magenta);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, 1f, 768))
                _agent.speed = 1.2f;
            else
                _agent.speed = 2;
        }

        private void GotoNextPoint()
        {
            var debugPath = GetComponent<DebugPath>();

            if (!RandomPosition)
            {
                _pos = (_pos + 1) % positions.Length;
                _agent.destination = positions[_pos].position;
            }
            else
            {
                var nearEvents = Physics.OverlapSphere(transform.position, 15, 1024);
                if (nearEvents.Length > 0 && Status == NpcStatus.Walking && Random.value > 0.85)
                {
                    var transformPosition = nearEvents[new System.Random().Next(nearEvents.Length)].transform.position;
                    float angle = Random.Range(0, Mathf.PI);
                    _agent.destination = transformPosition + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * 1.2f;
                    SetStatus(NpcStatus.GoingToEvent);
#if DEBUG
                    debugPath.target = _agent.destination;
                    debugPath.color = Color.blue;
#endif
                }
                else
                {
                    FindRandomDestination(SearchRadius);
#if DEBUG
                    debugPath.target = _agent.destination;
                    debugPath.color = Color.red;
#endif
                }
            }
        }

        public void FindRandomDestination(float range = 0)
        {
            if (range == 0)
                range = SearchRadius;
            var navMeshPath = new NavMeshPath();
            Vector3 newPos;
            do
            {
                newPos = transform.position + Random.insideUnitSphere * range;
                if (!FindInSphere)
                    newPos.y = ParentPosition.y;

                
            } while (NavMesh.CalculatePath(ParentPosition, newPos, NavMesh.AllAreas, navMeshPath) && navMeshPath.status != NavMeshPathStatus.PathComplete);

            // Vector3 insideUnitSphere;
            // NavMeshHit hit;
            // do
            // {
            //     insideUnitSphere = Random.insideUnitSphere * 15f + transform.position;
            // } while (NavMesh.SamplePosition(insideUnitSphere, out hit, 1.0f, NavMesh.AllAreas));

            // _agent.destination = hit.position;
            _agent.destination = newPos;
        }
    }
}