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
        [SerializeField] private AssignTarget assignTarget;
        [SerializeField] private GameObject killFeed;
        [SerializeField] private GameObject killFeedTemplate;
        [SerializeField] private Text textIndicator;
        private int _roundCount;


        void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void SpectatorMode()
        {
            var player = igs.localPlayer;
            _spectator = Instantiate(spectatorPrefab);
            _spectator.transform.position = player.transform.position;
            _spectator.transform.rotation = player.transform.rotation;
        }

        private void SetDeadCustomProp()
        {
            Hashtable deadCustomProp = new Hashtable {{"dead", true},{"deathCount",igs.deathCount}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(deadCustomProp);
        }

        private void SetKillCustomProp()
        {
            Hashtable killCustomProp = new Hashtable {{"killCount", igs.killCount}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(killCustomProp);
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
                scoreManager.AlivePoints();
                if (_spectator != null) Destroy(_spectator);
                playerManager.RespawnPlayers();
                if (PhotonNetwork.IsMasterClient) npcManager.StartRespawnCoroutine();
                if (igs.target != null) assignTarget.KilledTarget();
                if (_roundCount == maxRound)
                {
                    timerManager.enabled = false;
                    endGameManager.StartEndRoundCoroutine();
                }
                return; 
            }
            var killedPhotonView = PhotonView.Find(Convert.ToInt32(photonEvent.CustomData));
            var killer = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
            var amKiller = killer.Equals(PhotonNetwork.LocalPlayer);
            if (photonEvent.Code == EventManager.KilledPlayerEventCode)
            {
                var killed = killedPhotonView.Owner;
                var template = Instantiate(killFeedTemplate, killFeed.transform);
                template.GetComponent<KillFeed>().CreateKillFeedEntry(killer.NickName,killed.NickName);
                Debug.Log("Player died: " + killed.NickName);
                var killedGo = killedPhotonView.gameObject;
                killedGo.SetActive(false);
                if (killed.Equals(PhotonNetwork.LocalPlayer))
                {
                    igs.deathCount++;
                    SetDeadCustomProp();
                    SpectatorMode();
                    StartCoroutine(EnableDeathHud(killer.NickName));
                    //TODO: Implement spectator system
                }
                timerManager.AccTimer();
                var isTarget = igs.target != null && killed.Equals(igs.target.GetPhotonView().Owner);
                if (amKiller && isTarget)
                {
                    StartCoroutine(TextIndicator("You killed your target!"));
                    igs.killCount++;
                    SetKillCustomProp();
                    assignTarget.KilledTarget();
                }
                if (amKiller && !isTarget) StartCoroutine(TextIndicator("You killed the wrong player!"));
                scoreManager.KilledPlayer(amKiller, isTarget);
            }
            else if (photonEvent.Code == EventManager.KilledNpcEventCode)
            {
                var killedNPCGo = killedPhotonView.gameObject;
                Debug.Log("NPC died: " + killedNPCGo);
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(killedNPCGo);
                npcManager.dead++;
                var template = Instantiate(killFeedTemplate, killFeed.transform);
                template.GetComponent<KillFeed>().CreateKillFeedEntry(killer.NickName);
                if (killer.Equals(PhotonNetwork.LocalPlayer))
                {
                    StartCoroutine(TextIndicator("You killed an NPC!"));
                    scoreManager.KilledNpc();
                }
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

        IEnumerator TextIndicator(string text)
        {
            textIndicator.text = text;
            var textIndicatorColor = textIndicator.color;
            textIndicatorColor.a = 1;
            yield return new WaitForSeconds(5);
            textIndicator.text = "";
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