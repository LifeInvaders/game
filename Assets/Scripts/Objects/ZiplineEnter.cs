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
            translateVector3 = endPos.transform.position - startPos.transform.position;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer") && other.GetComponent<PlayerEvent>().humanTask != HumanTasks.Zipline) // si collision avec autre joueur
            {
                // change all the player's parameters
                PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
                player.GetComponent<PlayerEvent>().humanTask = HumanTasks.Zipline;
                var rigidbody = other.gameObject.GetComponent<Rigidbody>();
                rigidbody.useGravity = false; // on désactive la gravité du joueur
                rigidbody.isKinematic = true;
                player.SetRotateBool(false);
                player.SetMoveBool(false); // on désactive les mouvements
                player.CheckIfRunning();
                player.transform.position =
                    new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
                player.transform.rotation = transform.rotation;
                other.GetComponent<Animator>().SetTrigger("zipline");

                // spawn a sword in the player's hand
                var sword = other.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/sword");
                sword.gameObject.SetActive(true);
                // var rotation = sword.transform.rotation;
                // rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.y-60);
                // sword.transform.rotation = rotation;

                // start to move
                StartCoroutine(CheckforJump(other.gameObject));
                StartCoroutine(MovingOn(other.gameObject));
            }
        }

        private IEnumerator CheckforJump(GameObject player)
        {
            var controler = player.GetComponent<PlayerControler>();
            while (controler.GetAxis().y >= 0)
            {
                yield return new WaitForEndOfFrame();
            }

            var playerEvent = player.GetComponent<PlayerEvent>();
            controler.enabled = true;
            Rigidbody rigidbody = player.gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = true; // on réactive la gravité du joueur
            controler.SetRotateBool(true);
            controler.SetMoveBool(true); // on réactive les mouvements
            // controler.CheckIfRunning();
            rigidbody.isKinematic = false;
            player.GetComponent<Animator>().SetTrigger("Default");
            playerEvent.humanTask = HumanTasks.Nothing;
            var sword = player.transform.Find(
                "Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/sword");
            sword.gameObject.SetActive(false);
            
        }

        private IEnumerator MovingOn(GameObject player)
        {
            var playerEvent = player.GetComponent<PlayerEvent>();
            while (playerEvent.humanTask == HumanTasks.Zipline)
            {
                player.transform.Translate(translateVector3.normalized * Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}