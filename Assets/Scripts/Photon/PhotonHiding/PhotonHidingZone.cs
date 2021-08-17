using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace People.NPC.Hiding
{
    public abstract class PhotonHidingZone : MonoBehaviour
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

        public string GetStatus()
        {
            switch (anim)
            {
                case NpcStatus.Talking:
                    return "talk";
                case NpcStatus.Praying:
                    return "pray";
                default:
                    return "";
            }
        }
        void Start()
        {
            //Random.InitState(gameObject.GetPhotonView().ViewID);
            //if (NumberOfNpc == 0) NumberOfNpc = Random.Range(2, 4);
            
            NPCs = new List<GameObject>();
            NpcInTheZone = new GameObject[NumberOfNpc];
            nextPlace = 0;
            if (NumberOfNpcInTheZone < NumberOfNpc && !_searching)
            {
                StartCoroutine(FindNPC(1));
            }
        }

        protected abstract void WherePlaceNpc(PhotonNPC walkingNpc);

        private IEnumerator FindNPC(int waiting = 5)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;
            _searching = true;
            yield return new WaitForSeconds(waiting);
            var nearEvents = Physics.OverlapSphere(transform.position, 15, 512);

            PhotonNPC walkingNpc;
            int i = 0;
            for (; i < nearEvents.Length; i++)
            {
                walkingNpc = nearEvents[i].GetComponent<PhotonNPC>();
                if (walkingNpc.start)
                {
                    gameObject.GetPhotonView().RPC(nameof(AddNPC),RpcTarget.All,nearEvents[i].gameObject.GetPhotonView().ViewID,nextPlace);
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

        [PunRPC]
        public void AddNPC(int viewID,int nextPlace)
        {
            Debug.Log("NPC added to zone.");
            var npc = PhotonNetwork.GetPhotonView(viewID).gameObject.GetComponent<PhotonNPC>();
            NumberOfNpcInTheZone++;
            npc.ResetNpc();
            NpcInTheZone[nextPlace] = npc.gameObject;
            var anim = npc.GetComponent<Animator>();
            npc.gameObject.GetComponent<Animator>().SetBool("walk", true);
            npc.SetEventZone(gameObject);
            WherePlaceNpc(npc);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision detected. Checking for npc...");
            if (!other.gameObject.CompareTag("NPC")) return;
            Debug.Log("NPC entered zone. Checking if npc is in this zone...");
            var npc = other.GetComponent<PhotonNPC>();
            if (!npc.GetEventZone() == gameObject || npc.inZone) return;
            npc.inZone = true;
            Debug.Log("Changing animation and rotation");
            NPCs.Add(other.gameObject);
            StartCoroutine(RotateNPC(other.GetComponent<NavMeshAgent>(), other.transform));
            var anim = other.GetComponent<Animator>();
            anim.SetBool("walk", false);
            var talkingNpc = other.gameObject.GetComponent<TalkingNPC>(); 
            talkingNpc.enabled = true;
            talkingNpc.SetAnim(GetStatus());
            
            // other.transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
            other.transform.LookAt(transform, Vector3.up);

            if (NPCs.Count == NumberOfNpc)
            {
                StartCoroutine(RemoveNpc());
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
                    StartCoroutine(RemoveNpc(index, false));
                    break;
                }
        }

        private IEnumerator RemoveNpc(int index = 0, bool waiting = true)
        {
            if (waiting)
                yield return new WaitForSeconds(6);
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
                    NPCs[index].GetComponent<TalkingNPC>().enabled = false;
                    var walkingNpc = NPCs[index].GetComponent<PhotonNPC>();
                    walkingNpc.StartBootUp(5);
                    walkingNpc.SetEventZone(null);
                    walkingNpc.inZone = false;
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