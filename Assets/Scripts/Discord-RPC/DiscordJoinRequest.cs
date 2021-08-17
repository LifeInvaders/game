using System;
using System.Collections;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Discord_RPC
{
    public class DiscordJoinRequest : MonoBehaviour
    {
        private DiscordRpc.JoinRequest joinRequest;

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private TextMeshProUGUI timer;
        // IEnumerator GetTexture() {
        //     UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.my-server.com/image.png");
        //     yield return www.SendWebRequest();
        //
        //     Texture myTexture = DownloadHandlerTexture.GetContent(www);
        // }
        public void UpdateRequest(DiscordRpc.JoinRequest request)
        {
            joinRequest = request;
            text.text = request.username + "#" + request.discriminator;
        }
        private void Start()
        {
            StartCoroutine(WaitToIgnore());
        }

        private IEnumerator WaitToIgnore()
        {
           
            for (int i = 45; i > 0; i--)
            {
                timer.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Ignore);
            Destroy(gameObject);
        }

        public void AnswerYes()
        {
            StopCoroutine(WaitToIgnore());
            DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
            Destroy(gameObject);
        }

        public void AnswerNo()
        {
            StopCoroutine(WaitToIgnore());
            DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
            Destroy(gameObject);
        }
    }
}