using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Player;
using UnityEngine;

public class LobbySkinSelect : MonoBehaviour
{
    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            char[] charArray = {PlayerDatabase.Instance.Rank, PlayerDatabase.Instance.Gender, PlayerDatabase.Instance.Variant, PlayerDatabase.Instance.SkinColor};
            string searchName = new string(charArray);
            ApplySkin(searchName);
        }
        else if (gameObject.GetPhotonView().IsMine)
        {
            char[] charArray = {PlayerDatabase.Instance.Rank, PlayerDatabase.Instance.Gender, PlayerDatabase.Instance.Variant, PlayerDatabase.Instance.SkinColor};
            string searchName = new string(charArray);
            gameObject.GetPhotonView().RPC("ApplySkin",RpcTarget.AllBufferedViaServer, searchName);
        }
    }

    [PunRPC]
    private void ApplySkin(string searchName)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == searchName)
                child.gameObject.SetActive(true);
            else if (child.gameObject.activeSelf)
                child.gameObject.SetActive(false);
        }
    }
}
