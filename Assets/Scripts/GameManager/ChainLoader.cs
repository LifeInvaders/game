using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameManager
{
    public class ChainLoader : MonoBehaviourPunCallbacks
    {
        private Photon.Realtime.Player[] players;

        [SerializeField] private NpcManager npcManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private TimerManager timerManager;
        [SerializeField] private ListPlayers listPlayers;
        
        IEnumerator Start()
        {
            players = PhotonNetwork.PlayerList;
            Debug.Log("Settings status as ready");
            Hashtable ready = new Hashtable {{"ready", true}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(ready);
            Debug.Log("Waiting for all players to be ready...");
            yield return new WaitUntil(CheckReady);
            npcManager.enabled = true;
            Debug.Log("Waiting for npc activation...");
            yield return new WaitUntil(CheckNpcSyncTime);
            playerManager.enabled = true;
            playerManager.StartCoroutine(playerManager.LatePick());
            Debug.Log("Waiting for player spawning...");
            yield return new WaitUntil(CheckAssignViewID);
            if (PhotonNetwork.IsMasterClient)
                yield return new WaitForSeconds(1);
            timerManager.enabled = true;
            listPlayers.enabled = true;
        }
        
        bool CheckNpcSyncTime()
        {
            return PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("npcDone", out var time) && PhotonNetwork.Time >= Convert.ToDouble(time);
        }

        bool CheckAssignViewID()
        {
            foreach (Photon.Realtime.Player player in players)
            {
                if (!player.CustomProperties.TryGetValue("viewID",out var viewID))
                    return false;
            }
            return true;
        }
        bool CheckReady()
        {
            foreach (Photon.Realtime.Player player in players)
            {
                if (!player.CustomProperties.TryGetValue("ready", out var ready))
                    return false;
            }
            return true;
        }
    }
}