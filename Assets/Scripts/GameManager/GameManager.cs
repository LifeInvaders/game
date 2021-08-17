using System;
using System.Collections;
using ExitGames.Client.Photon;
using People.NPC;
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
        [SerializeField] private GameObject poisonKill;
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
            if (photonEvent.Code != EventManager.EndRoundEventCode &&
                photonEvent.Code != EventManager.KilledPlayerEventCode &&
                photonEvent.Code != EventManager.KilledNpcEventCode) return;
            if (photonEvent.Code == EventManager.EndRoundEventCode)
            {
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
            var killedPhotonView = PhotonView.Find(Convert.ToInt32(((object[])photonEvent.CustomData)[0]));
            var anim = Convert.ToInt32(((object[]) photonEvent.CustomData)[1]);
            var killer = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
            var killerGo = PhotonView.Find(Convert.ToInt32(killer.CustomProperties["viewID"])).gameObject;
            var amKiller = killer.Equals(PhotonNetwork.LocalPlayer);
            if (photonEvent.Code == EventManager.KilledPlayerEventCode)
            {
                var killed = killedPhotonView.Owner;
                if ((bool) killed.CustomProperties["dead"]) return;
                var template = Instantiate(killFeedTemplate, killFeed.transform);
                template.GetComponent<KillFeed>().CreateKillFeedEntry(killer.NickName,killed.NickName);
                Debug.Log("Player died: " + killed.NickName);
                var killedGo = killedPhotonView.gameObject;
                StartCoroutine(PlayerFinisher(killerGo,killedGo,anim));
                if (killed.Equals(PhotonNetwork.LocalPlayer))
                {
                    igs.deathCount++;
                    SetDeadCustomProp();
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
                scoreManager.KilledPlayer(amKiller, isTarget,anim == -1);
            }
            else if (photonEvent.Code == EventManager.KilledNpcEventCode)
            {
                var killedNPCGo = killedPhotonView.gameObject;
                NpcFinisher(killerGo,killedNPCGo,anim);
                Debug.Log("NPC died: " + killedNPCGo);
                killedNPCGo.GetComponent<PhotonNPCEvent>().Death();
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

        IEnumerator PlayerFinisher(GameObject killer, GameObject killed,int randFinisher = 0)
        {
            var killerMesh = killer.GetComponentInChildren<SkinnedMeshRenderer>();
            var killedMesh = killed.GetComponentInChildren<SkinnedMeshRenderer>();
            var poison = randFinisher == -1;
            GameObject g;
            if (poison) g = Instantiate(poisonKill, killed.transform.position, killed.transform.rotation);
            else g = Instantiate(finishers[randFinisher], killer.transform.position, killer.transform.rotation);
            Finisher finisher = g.GetComponent<Finisher>();
            finisher.SetHumans(killerMesh,
                killedMesh);
            g.GetComponent<Finisher>().player = poison ? killed : killer;
            g.GetComponent<Finisher>().victim = killed;
            finisher.camera.enabled = !poison && igs.localPlayer.Equals(killer) || igs.localPlayer.Equals(killed);
            if (!poison) killerMesh.enabled = false;
            killedMesh.enabled = false;
            killer.GetComponent<PlayerControler>().SetMoveBool(false);
            yield return new WaitUntil(() => finisher.animFinished);
            killed.SetActive(false);
            killedMesh.enabled = true;
            if (killed.Equals(igs.localPlayer)) SpectatorMode();
        }

        IEnumerator TextIndicator(string text)
        {
            textIndicator.text = text;
            var textIndicatorColor = textIndicator.color;
            textIndicatorColor.a = 1;
            yield return new WaitForSeconds(5);
            textIndicator.text = "";
        }

        public void NpcFinisher(GameObject killer, GameObject killed,int randFinisher = 0)
        {
            var killerMesh = killer.GetComponentInChildren<SkinnedMeshRenderer>();
            var poison = randFinisher == -1;
            GameObject g;
            if (poison) g = Instantiate(poisonKill, killed.transform.position, killed.transform.rotation);
            else g = Instantiate(finishers[randFinisher], killer.transform.position, killer.transform.rotation);
            Finisher finisher = g.GetComponent<Finisher>();
            finisher.SetHumans(killerMesh,
                killed.GetComponentInChildren<SkinnedMeshRenderer>());
            finisher.player = poison ? killed : killer;
            finisher.victim = killed;
            finisher.camera.enabled = !poison && igs.localPlayer.Equals(killer);
            if (poison) return;
           killerMesh.enabled = false;
            killer.GetComponent<PlayerControler>().SetMoveBool(false);
        }
    }
}