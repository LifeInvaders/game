using System;
using System.Collections;
using ExitGames.Client.Photon;
using People.Player;
using Photon.Compression;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameManager
{
    public class GameManager : MonoBehaviour, IOnEventCallback
    {

        [SerializeField] private GameObject spectatorPrefab;
        private GameObject _spectator;
        [SerializeField] private GameObject deathDisplay;
        [SerializeField] private InGameStats igs;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TimerManager timerManager;
        [SerializeField] private GameObject[] finishers;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private NpcManager npcManager;
        [SerializeField] private EndGameManager endGameManager;
        [SerializeField] private int maxRound;
        private int _roundCount;


        void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void SpectatorMode()
        {
            var player = igs.localPlayer;
            if (_spectator == null)
                _spectator = Instantiate(spectatorPrefab);
            else _spectator.SetActive(true);
            _spectator.transform.position = player.transform.position;
            _spectator.transform.rotation = player.transform.rotation;

        }

        private void SetDeadCustomProp()
        {
            Hashtable deadCustomProp = new Hashtable {{"dead", true}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(deadCustomProp);
        }
        IEnumerator EnableDeathHud(string killer)
        {
            deathDisplay.GetComponent<Text>().text = "Killed by " + killer;
            deathDisplay.SetActive(true);
            yield return new WaitForSeconds(5);
            deathDisplay.SetActive(false);
        }
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventManager.EndRoundEventCode)
            {
                Debug.Log("Round ended!");
                _roundCount++;
                playerManager.RespawnPlayers();
                if (PhotonNetwork.IsMasterClient) npcManager.StartRespawnCoroutine();
                if (_roundCount == maxRound)
                {
                    timerManager.enabled = false;
                    endGameManager.StartEndRoundCoroutine();
                }
                if (_spectator != null && _spectator.activeSelf) _spectator.SetActive(false);
                return; 
            }
            var killedPhotonView = PhotonView.Find(Convert.ToInt32(photonEvent.CustomData));
            var killer = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
            var amKiller = killer.Equals(PhotonNetwork.LocalPlayer);
            if (photonEvent.Code == EventManager.KilledPlayerEventCode)
            {
                var killed = killedPhotonView.Owner;
                Debug.Log("Player died: " + killed.NickName);
                var killedGo = killedPhotonView.gameObject;
                killedGo.SetActive(false);
                if (killed.Equals(PhotonNetwork.LocalPlayer))
                {
                    igs.deathCount++;
                    SetDeadCustomProp();
                    StartCoroutine(EnableDeathHud(killer.NickName));
                    //TODO: Implement spectator system
                }
                timerManager.AccTimer();
                var isTarget = killed.Equals(igs.target.GetPhotonView().Owner);
                if (amKiller && isTarget) igs.killCount++;
                scoreManager.KilledPlayer(amKiller, isTarget);
            }
            else if (photonEvent.Code == EventManager.KilledNpcEventCode)
            {
                if (killer.Equals(PhotonNetwork.LocalPlayer)) scoreManager.KilledNpc();
                var killedNPCGo = killedPhotonView.gameObject;
                Debug.Log("NPC died: " + killedNPCGo);
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(killedNPCGo);
                npcManager.dead++;
            }
        }

        public void PlayerFinisher(int randFinisher,GameObject killer, GameObject killed)
        {
            var g = Instantiate(finishers[randFinisher], transform.position, transform.rotation);
            Finisher finisher = g.GetComponent<Finisher>();
            finisher.SetHumans(killer.GetComponentInChildren<SkinnedMeshRenderer>(),
                killed.GetComponentInChildren<SkinnedMeshRenderer>());
            g.GetComponent<Finisher>().player = killer;
            g.GetComponent<Finisher>().victim = killed;
            killer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            killer.GetComponent<PlayerControler>().SetMoveBool(false);
        }

        public void NpcFinisher(int randFinisher, GameObject killer, GameObject killed)
        {
            var g = Instantiate(finishers[randFinisher], transform.position, transform.rotation);
            Finisher finisher = g.GetComponent<Finisher>();
            finisher.SetHumans(killer.GetComponentInChildren<SkinnedMeshRenderer>(),
                killed.GetComponentInChildren<SkinnedMeshRenderer>());
            g.GetComponent<Finisher>().player = killer;
            g.GetComponent<Finisher>().victim = killed;
            killer.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            killer.GetComponent<PlayerControler>().SetMoveBool(false);
        }
    }
}