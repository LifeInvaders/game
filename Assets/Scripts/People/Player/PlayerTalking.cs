using System;
using People.NPC.Hiding;
using UnityEngine;

namespace People.Player
{
    
    public class PlayerTalking : MonoBehaviour
    {
        [SerializeField] private GameObject triggerGameObject;
        private HidingZone _hidingZone;
        public void Start()
        {
            _hidingZone = triggerGameObject.GetComponent<HidingZone>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Vector2 axis = other.gameObject.GetComponent<PlayerControler>().GetAxis();
                other.gameObject.GetComponent<Animator>()
                    .SetBool(_hidingZone.GetStatus(), axis.Equals(Vector2.zero));
            }
        }

    }
}
