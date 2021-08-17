using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Discord_RPC
{
    public class DiscordEvent : MonoBehaviour
    {
        [SerializeField] private GameObject joinMessage;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject joinRequestNotif;
      
        public void Join(string idRoom)
        {
            errorMessage.SetActive(false);
            joinMessage.SetActive(true);
            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            StartCoroutine(WaitWhileNotReady(idRoom.Remove(idRoom.Length - 1)));
            
        }

        private IEnumerator WaitWhileNotReady(string idRoom)
        {
            yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
            if (PhotonNetwork.JoinRoom(idRoom))
            {
                joinMessage.SetActive(false);
            }
            else
            {
                joinMessage.GetComponentInChildren<TextMeshProUGUI>().text =
                    "Error : Can't connect to the room " + idRoom;
                joinMessage.SetActive(false);
                errorMessage.SetActive(true);

                yield return new WaitForSeconds(3);
                errorMessage.SetActive(false);
            }
            
        }
        
        public void JoinRequest(DiscordRpc.JoinRequest joinRequest)
        {
            Debug.Log("join test");
            var obj = Instantiate(joinRequestNotif, canvas.transform).GetComponent<DiscordJoinRequest>();
            obj.UpdateRequest(joinRequest);
            // DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        }

        public void HasAnswered()
        {
            
        }
        

    }
}