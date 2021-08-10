using System.Collections;
using People;
using People.Player;
using UnityEngine;

namespace Objects
{
    public class ZiplineEnter : MonoBehaviour
    {
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;
        [SerializeField] private float speed;
        [SerializeField] private LineRenderer _lineRenderer;
        private Vector3 translateVector3;

        public void Start()
        {
            // translateVector3 = transform.parent.TransformDirection(_lineRenderer.GetPosition(1)) -  transform.parent.TransformDirection(_lineRenderer.GetPosition(0));
            translateVector3 = (_lineRenderer.GetPosition(1) - _lineRenderer.GetPosition(0)).normalized;
            Debug.Log(translateVector3);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer") &&
                other.GetComponent<PlayerEvent>().humanTask == HumanTasks.Nothing) // si collision avec autre joueur
            {
                // change all the player's parameters
                PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
                player.GetComponent<HumanEvent>().humanTask = HumanTasks.Zipline;
                var rigidbody = other.gameObject.GetComponent<Rigidbody>();
                rigidbody.useGravity = false; // on désactive la gravité du joueur
                rigidbody.isKinematic = true;
                player.SetRotateBool(false);
                player.SetMoveBool(false); // on désactive les mouvements
                player.CheckIfRunning();
                player.transform.position =
                    new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
                other.transform.LookAt(_lineRenderer.GetPosition(1));
                //player.transform.rotation = transform.rotation;
                var animator = other.GetComponent<Animator>();
                animator.SetTrigger("zipline");

                // spawn a sword in the player's hand
                var sword = other.transform.Find(
                    "Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/sword");
                sword.gameObject.SetActive(true);
                // var rotation = sword.transform.rotation;
                // rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.y-60);
                // sword.transform.rotation = rotation;

                // start to move
                StartCoroutine(MovingOn(other.gameObject));
            }
        }

        private IEnumerator Leave(GameObject player)
        {
            var playerControler = player.GetComponent<PlayerControler>();

            var playerEvent = player.GetComponent<HumanEvent>();
            playerControler.enabled = true;
            Rigidbody rigidbody = player.gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = true; // on réactive la gravité du joueur
            playerControler.SetRotateBool(true);
            playerControler.SetMoveBool(true); // on réactive les mouvements
            rigidbody.isKinematic = false;
            var animator = player.GetComponent<Animator>();
            animator.SetTrigger("jumping");
            playerEvent.humanTask = HumanTasks.Nothing;
            var sword = player.transform.Find(
                "Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/sword");
            sword.gameObject.SetActive(false);

            yield return new WaitUntil(() => Physics.Raycast(player.transform.position, Vector3.down, 1.5f));
            
            animator.SetTrigger("Default");

        }

        private IEnumerator MovingOn(GameObject player)
        {
            var playerEvent = player.GetComponent<HumanEvent>();
            var playerControler = player.GetComponent<PlayerControler>();
            while (playerEvent.humanTask == HumanTasks.Zipline)
            {
                player.transform.position += translateVector3 * (Time.deltaTime * speed);
                if (playerControler.IsJumping)
                {
                   StartCoroutine( Leave(player));
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}