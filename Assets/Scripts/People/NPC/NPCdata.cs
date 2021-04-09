using UnityEngine;

namespace People.NPC
{
    public class NPCdata : MonoBehaviour
    {
        [SerializeField] protected NpcStatus Status;
        public NpcStatus GetStatus() => Status;
        public void SetStatus(NpcStatus status)
        {
            switch (Status)
            {
                case NpcStatus.GoingToEvent:
                case NpcStatus.Walking:
                    GetComponent<WalkingNPC>().enabled = false;
                    GetComponent<Animator>().SetBool("walk", false);

                    break;
                case NpcStatus.Talking:
                    GetComponent<TalkingNPC>().enabled = false;
                    GetComponent<Animator>().SetBool("talk", false);
                    break;
                // case NpcStatus.GoingToEvent:
            }

            switch (status)
            {
                case NpcStatus.GoingToEvent:
                case NpcStatus.Walking:
                    GetComponent<WalkingNPC>().enabled = true;
                    GetComponent<Animator>().SetBool("walk", true);
                    // GetComponent<WalkingNPC>()
                    break;
                case NpcStatus.Talking:
                    GetComponent<TalkingNPC>().enabled = true;
                    GetComponent<Animator>().SetBool("talk", true);

                    break;
            }

            Status = status;
        }
    }
}