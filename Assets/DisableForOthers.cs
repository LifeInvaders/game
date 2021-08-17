using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DisableForOthers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected && !transform.parent.gameObject.GetPhotonView().IsMine) gameObject.SetActive(false);
    }
}
