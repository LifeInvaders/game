using System.Collections;
using People.Player;
using UnityEngine;
using UnityEngine.AI;

namespace People
{
    public class WalkingNPC : MonoBehaviour
    {
        private Animator anim;
        private NavMeshAgent agent;
        public GameObject iaPoints;
        private Transform[] positions;
        public bool RandomPosition = false;
        private int pos = 0;
        private bool shoved = false;
        void Start()
        {
            positions = iaPoints.GetComponentsInChildren<Transform>();
            agent = GetComponent<NavMeshAgent>();
            // agent.autoBraking = false;
            anim = GetComponent<Animator>();

            anim.SetBool("walk", true);
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerControler>().Running() && !shoved)
            {
                agent.isStopped = true;
                shoved = true;
                Vector3 pos =GetComponent<CapsuleCollider>().ClosestPointOnBounds(other.gameObject.transform.position);
                bool right = pos.normalized.x < 0;
                bool forward = pos.normalized.z < 0;
                // transform.position -= Vector3.right * pos.normalized.x * .4f;

                anim.SetBool("mirror",right);
                anim.Play("shove");
                StartCoroutine(WaitCoroutine());

            }
        }

        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(3);
            agent.isStopped = false;
            shoved = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }

        private void GotoNextPoint()
        {
            if (RandomPosition)
                agent.destination = positions[Random.Range(0, positions.Length)].position;
            else
            {
                pos = (pos + 1) % positions.Length;
                agent.destination = positions[pos].position;
            }
        }
    }
}