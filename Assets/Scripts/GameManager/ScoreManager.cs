using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class ScoreManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private InGameStats igs;
        
        [SerializeField] private int baseKillPoints;
        [SerializeField] private int killStealPoints;
        [SerializeField] private int npcMalusPoints;
        [SerializeField] private int killDowngradePoints;
        [SerializeField] private int alivePoints;
        [SerializeField] private GameObject scoreHud;
        [NonSerialized]public List<Photon.Realtime.Player> scoreBoard;
        private int _roundKills;

        void Start()
        {
            PhotonNetwork.LocalPlayer.SetScore(0);
            scoreBoard = PhotonNetwork.PlayerList.ToList();
            UpdateHud();
        }

        public void KilledNpc()
        {
            PhotonNetwork.LocalPlayer.AddScore(npcMalusPoints);
        }

        public void ResetKills() => _roundKills = 0;
        public void KilledPlayer(bool amKiller, bool isTarget,bool poison = false)
        {
            if (isTarget)
            {
                if (amKiller) PhotonNetwork.LocalPlayer.AddScore((poison ? 2 : 0) + baseKillPoints - _roundKills * killDowngradePoints);
                else PhotonNetwork.LocalPlayer.AddScore(killStealPoints);
            }
            _roundKills++;
        }

        public void AlivePoints()
        {
            if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("dead", out var dead))return;
            if (!(bool)dead) PhotonNetwork.LocalPlayer.AddScore(alivePoints);
        }

        public void UpdateHud()
        {
            scoreHud.GetComponent<Text>().text = "Score: " + PhotonNetwork.LocalPlayer.GetScore();
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PunPlayerScores.PlayerScoreProp) && targetPlayer.IsLocal)
                UpdateHud();
        }
        



        private int ScoreSort(Photon.Realtime.Player p1, Photon.Realtime.Player p2)
        {
            int p1S = p1.GetScore();
            int p2S = p2.GetScore();
            if (p1S == p2S) return 0;
            if (p1S < p2S) return 1;
            return -1;
        }

        public void DeathSortFunc() => scoreBoard.Sort(DeathSort);

        private int DeathSort(Photon.Realtime.Player p1, Photon.Realtime.Player p2)
        {
            int p1D = (int)p1.CustomProperties["deathCount"];
            int p2D = (int) p2.CustomProperties["deathCount"];
            if (p1D == p2D) return 0;
            if (p1D < p2D) return 1;
            return -1;
        }

        public int GetRank(Photon.Realtime.Player player = null)
        {
            if (player == null) player = PhotonNetwork.LocalPlayer;
            return scoreBoard.IndexOf(player) + 1;
        }
    }
}