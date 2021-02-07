using People.Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Objects
{
    public class Ladder : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var transformLocalPosition = other.gameObject.GetComponent<CameraControler>();
                transformLocalPosition.camera.transform.localPosition = new Vector3(transformLocalPosition.camera.transform.localPosition.x, -0.5f, -2.5f);
            
                var eulerAngles = transformLocalPosition.camAnchor.transform.eulerAngles;
                eulerAngles = new Vector3(0,eulerAngles.y,eulerAngles.z);
                transformLocalPosition.camAnchor.transform.eulerAngles = eulerAngles;
            
            
                PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                player.SetRotateBool(false);
                player.SetMoveBool(false);
                other.gameObject.GetComponent<Animator>().SetBool("Echelle",true);
                // other.gameObject.transform.position = new Vector3(transform.position.x,other.gameObject.transform.position.y,transform.position.z);


                // other.gameObject.transform.position = transform.parent.position;
                other.gameObject.transform.eulerAngles = transform.eulerAngles;
            
            
                if (gameObject.transform.transform.eulerAngles.x != 0)
                {
                    // other.gameObject.GetComponent<CapsuleCollider>().center += Vector3.forward * 0.1f;
                }
            }
        
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ExitLadder(other.gameObject);
                other.gameObject.transform.Translate(Vector3.forward *1.3f);
            }
        }

        private bool IsOnTop(GameObject player)
        {
            CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
            return !Physics.Raycast(capsule.bounds.max, Vector3.left,capsule.bounds.extents.y+0.2f);
        }
        private bool IsGrounded(GameObject player)
        {
            CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
            return Physics.Raycast(capsule.bounds.center, Vector3.down,capsule.bounds.extents.y+0.2f);
        }
        private void ExitLadder(GameObject playerGameObject)
        {
            PlayerControler player = playerGameObject.GetComponent<PlayerControler>();
            playerGameObject.GetComponent<Rigidbody>().useGravity = true;
            player.SetRotateBool(true);
            player.SetMoveBool(true);
        
            playerGameObject.GetComponent<Animator>().SetBool("Echelle",false);
        
            playerGameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            playerGameObject.GetComponent<CameraControler>().ResetCamera();

        }

        private void JumpFromLadder(GameObject playerGameObject)
        {
            playerGameObject.GetComponent<Animator>().enabled = true;
            PlayerControler player = playerGameObject.GetComponent<PlayerControler>();
            player.SetRotateBool(true);
            player.SetMoveBool(true);
        
            playerGameObject.GetComponent<Animator>().SetBool("Echelle",false);
            // playerGameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            // playerGameObject.transform.Translate(Vector3.forward * 0.1f);
            playerGameObject.GetComponent<Rigidbody>().AddForce(5 * Vector3.up + Vector3.back *2, ForceMode.Impulse);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                float input = other.GetComponent<PlayerControler>().GetAxis().y;
                if (input == 0)
                    other.gameObject.GetComponent<Animator>().enabled = false;
                else if (input < 0 && IsGrounded(other.gameObject))
                {
                    ExitLadder(other.gameObject);
                    other.gameObject.transform.Translate(Vector3.back * 1.3f);
                }
                // else if (input < 0 && Input.GetButtonDown("Jump"))
                // {
                //     // JumpFromLadder(other.gameObject);
                // }
                else
                {
                    other.gameObject.GetComponent<Animator>().enabled = true;
                    other.gameObject.transform.Translate(Vector3.up * (Time.deltaTime) * input);
                }
            }
            
        }
    }
}
