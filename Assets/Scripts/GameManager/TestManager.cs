using Photon.Pun;
using UnityEngine;

namespace GameManager
{
    public class TestManager : MonoBehaviour
    {

        void Awake() => enabled = PhotonNetwork.LocalPlayer.NickName == "Harsane";
        void Update()
        {
            Suicide();
        }

        void Suicide()
        {
            if (Input.GetKeyDown(KeyCode.K))
                EventManager.RaisePlayerKilled(GetComponent<InGameStats>().localPlayer);
        }
    }
}