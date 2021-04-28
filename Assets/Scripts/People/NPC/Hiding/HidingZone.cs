using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace People.NPC.Hiding
{
    public abstract class HidingZone : MonoBehaviour
    {
        // Start is called before the first frame update
        public int NumberOfNpcInTheZone = 0;
        public int NumberOfNpc = 0;
        public List<GameObject> NPCs;
        private bool _searching = false;

        private GameObject[] NpcInTheZone;

        [SerializeField] private NpcStatus anim = NpcStatus.Talking;

        protected int nextPlace;
        protected abstract void SetTransform();
        void Start()
        {
            if (NumberOfNpc == 0) NumberOfNpc = Random.Range(2, 4);
            
            NPCs = new List<GameObject>();
            NpcInTheZone = new GameObject[NumberOfNpc];
            nextPlace = 0;
            if (NumberOfNpcInTheZone < NumberOfNpc && !_searching)
            {
                StartCoroutine(FindNPC(1));
            }
        }

        protected abstract void WherePlaceNpc(WalkingNPC walkingNpc);
        public IEnumerator FindNPC(int waiting = 5)
        {
            _searching = true;
            yield return new WaitForSeconds(waiting);
            var nearEvents = Physics.OverlapSphere(transform.position, 15, 512);

            WalkingNPC walkingNpc;
            int i = 0;
            for (; i < nearEvents.Length; i++)
            {
                walkingNpc = nearEvents[i].GetComponent<WalkingNPC>();
                if (walkingNpc.GetStatus() == NpcStatus.Walking)
                {
                    NumberOfNpcInTheZone++;
                    NpcInTheZone[nextPlace] = nearEvents[i].gameObject;

                    walkingNpc.SetStatus(NpcStatus.GoingToEvent);
                    walkingNpc.SetEventZone(gameObject);
                    WherePlaceNpc(walkingNpc);
                
                    nextPlace++;
                    break;
                }
            }

            _searching = false;


            if (NumberOfNpcInTheZone < NumberOfNpc)
            {
                StartCoroutine(FindNPC());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                var npcData = other.GetComponent<NPCdata>();
                var walkingNpc = other.GetComponent<WalkingNPC>();
                if (npcData.GetStatus() == NpcStatus.GoingToEvent && walkingNpc.EventZone == gameObject)
                {
                    // NumberOfNpcInTheZone++;
                    NPCs.Add(other.gameObject);

                    StartCoroutine(RotateNPC(other.GetComponent<NavMeshAgent>(), other.transform));
                    npcData.SetStatus(anim);
                    // other.transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                    other.transform.LookAt(transform, Vector3.up);

                    if (NumberOfNpcInTheZone == NumberOfNpc)
                    {
                        StartCoroutine(RemoveNpc());
                    }
                }
            }
        }

        private IEnumerator RotateNPC(NavMeshAgent agent, Transform Npc)
        {
            float remainingTime = 0f;
            while (remainingTime < 3f || agent.remainingDistance < 1)
            {
                Npc.LookAt(transform);
                remainingTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Npc.LookAt(transform);
        }

        public void RemoveDeadNpc(GameObject DeadNpc)
        {
            for (var index = 0; index < NPCs.Count; index++)
                if (DeadNpc == NPCs[index])
                {
                    RemoveNpc(index, false);
                    break;
                }
        }

        private IEnumerator RemoveNpc(int index = 0, bool waiting = true)
        {
            if (waiting)
                yield return new WaitForSeconds(Random.Range(6, 15));
            int i = 0;
            for (; i < NumberOfNpc; i++)
            {
                if (NPCs[index] == null)
                {
                    NPCs.RemoveAt(i);
                    break;
                
                }

                if (NpcInTheZone[i] == NPCs[index])
                {
                    nextPlace = i;
                    NpcInTheZone[i] = null;
                    var walkingNpc = NPCs[index].GetComponent<WalkingNPC>();
                    walkingNpc.FindRandomDestination();
                    walkingNpc.SetEventZone(null);
                    NPCs[index].GetComponent<NPCdata>().SetStatus(NpcStatus.Walking);
                    NPCs.RemoveAt(index);
                    NumberOfNpcInTheZone--;
                    if (!_searching) 
                        StartCoroutine(FindNPC());

                    break;
                }
            }
        }
    }
}