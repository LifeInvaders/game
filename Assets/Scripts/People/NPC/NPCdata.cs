using UnityEngine;

namespace People.NPC
{
    public class NPCdata : MonoBehaviour
    {
        [SerializeField] protected NpcStatus Status;
        public NpcStatus GetStatus() => Status;

        public void SetStatus(NpcStatus status)
        {
            var talkingNpc = GetComponent<TalkingNPC>();
            var animator = GetComponent<Animator>();
            var walkingNpc = GetComponent<WalkingNPC>();
            switch (Status)
            {
                case NpcStatus.GoingToEvent:
                case NpcStatus.Walking:
                
                    walkingNpc.enabled = false;
                    animator.SetBool("walk", false);

                    break;
                case NpcStatus.Talking:
                    talkingNpc.enabled = false;
                    animator.SetBool("talk", false);
                    break;
                case NpcStatus.Praying:
                    talkingNpc.enabled = false;
                    animator.SetBool("pray", false);
                    break;
                // case NpcStatus.GoingToEvent:
            }

            switch (status)
            {
                case NpcStatus.GoingToEvent:
                case NpcStatus.Walking:
                    walkingNpc.enabled = true;
                    animator.SetBool("walk", true);
                    break;
                case NpcStatus.Talking:
                    talkingNpc.enabled = true;
                    talkingNpc.SetAnim("talk");
                    break;
                case NpcStatus.Praying:
                    talkingNpc.enabled = true;
                    talkingNpc.SetAnim("pray");
                    break;
            }

            Status = status;
        }
    }
}