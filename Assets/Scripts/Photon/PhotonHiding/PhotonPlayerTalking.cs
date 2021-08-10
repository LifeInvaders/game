using System;
using People.NPC.Hiding;
using UnityEngine;

namespace People.Player
{
    public class PhotonPlayerTalking : MonoBehaviour
    {
        [SerializeField] private GameObject triggerGameObject;
        private PhotonHidingZone _hidingZone;

        public void Start()
        {
            _hidingZone = triggerGameObject.GetComponent<PhotonHidingZone>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer"))
            {
                Vector2 axis = other.GetComponent<PlayerControler>().GetAxis();
                var animator = other.GetComponent<Animator>();
                var playerEvent = other.GetComponent<HumanEvent>();
                if (axis != Vector2.zero)
                {
                    if (playerEvent.humanTask == HumanTasks.Talking)
                    {
                        animator.SetTrigger("Default");
                        playerEvent.humanTask = HumanTasks.Nothing;
                    }
                }
                else if (playerEvent.humanTask == HumanTasks.Nothing)
                {
                    animator.SetTrigger(_hidingZone.GetStatus());
                    playerEvent.humanTask = HumanTasks.Talking;
                }
            }
        }
    }
}