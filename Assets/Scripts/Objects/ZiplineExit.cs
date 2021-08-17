using People;
using People.Player;
using UnityEngine;

namespace Objects
{
    public class ZiplineExit : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer")) // si collision avec autre joueur
            {
                var playerEvent = other.GetComponent<HumanEvent>();
                if (playerEvent.humanTask == HumanTasks.Zipline)
                {
                    PlayerControler controler = other.GetComponent<PlayerControler>();
                    controler.enabled = true;
                    other.gameObject.GetComponent<Rigidbody>().useGravity = true; // on réactive la gravité du joueur
                    controler.SetRotateBool(true);
                    controler.SetMoveBool(true); // on réactive les mouvements
                    // controler.CheckIfRunning();
                    other.GetComponent<Rigidbody>().isKinematic = false;
                    other.GetComponent<Animator>().SetTrigger("Default");
                    playerEvent.humanTask = HumanTasks.Nothing;
                    var sword = other.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/sword");
                    sword.gameObject.SetActive(false);
                }
            }
        }
    }
}