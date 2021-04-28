using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace People.Player
{
    public class PlayerEvent : HumanEvent,IOnEventCallback
    {
        public override void Death()
        {
        }
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventManager.KilledPlayerEventCode) return;
            var viewID = (int) photonEvent.CustomData;
            if (viewID == gameObject.GetPhotonView().ViewID) Death();
        }
    }
}