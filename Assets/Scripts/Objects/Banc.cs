using UnityEngine;

namespace Objects
{
    public class Banc : MonoBehaviour
    {
        public int nbPlayers;
        // private List<Player> _playerList;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("new player");
                // _playerList.Add(other.gameObject.);
            
                /*/*Set Sitting animation#1#
            anim.SetBool("sitting",true);
            capsule.center = new Vector3(capsule.center.x,0.95f,capsule.center.z);
            capsule.height = 1.1f;
            canRotate = false;
            transform.eulerAngles = col.gameObject.transform.eulerAngles + 180 * Vector3.up;
            transform.position = col.gameObject.transform.position;
            // transform.rotation.x += 180;
            // canMove = false;*/
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("remove player");
                // /*Go back to normal animation*/
                // canRotate = true;
                // // canMove = true;
                // anim.SetBool("sitting",false);
                // capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
                // capsule.height = 1.97f;
            }
        }
    }
}
