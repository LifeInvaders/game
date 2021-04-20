using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace GameManager
{
    public class ScoreManager
    {
        [SerializeField] private InGameStats igs;
        
        [SerializeField] private int baseKillPoints;
        [SerializeField] private int killStealPoints;
        [SerializeField] private int npcMalusPoints;
        [SerializeField] private int killDowngradePoints;
        
        private int roundKills;
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != EventManager.KilledEventCode) PlayerKilled(photonEvent);
            
        }

        private void PlayerKilled(EventData killEvent)
        {
            var killer = PhotonNetwork.LocalPlayer.Get(killEvent.Sender);
            var killed = (Photon.Realtime.Player) ((object[])killEvent.CustomData)[0];
            if (killed.Equals(igs.target.GetPhotonView().Owner))
            {
                if (killer.Equals(PhotonNetwork.LocalPlayer))
                    PhotonNetwork.LocalPlayer.AddScore(baseKillPoints - roundKills * killDowngradePoints);
                else
                    PhotonNetwork.LocalPlayer.AddScore(killStealPoints);
            }
        }
    }
}