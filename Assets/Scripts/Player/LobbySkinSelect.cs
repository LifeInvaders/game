using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Player;
using UnityEngine;

public class LobbySkinSelect : MonoBehaviour
{ 
    PlayerDatabase instance = PlayerDatabase.Instance;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            char[] charArray = {instance.Rank, instance.Gender, instance.Variant, instance.SkinColor};
            ApplySkin(charArray);
        }
        else if (gameObject.GetPhotonView().IsMine)
        {
            char[] charArray = {instance.Rank, instance.Gender, instance.Variant, instance.SkinColor};
            gameObject.GetPhotonView().RPC("ApplySkin",RpcTarget.All, charArray);
        }
    }

    [PunRPC]
    private void ApplySkin(char[] name)
    {
        string searchName = name.ToString();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == searchName)
                child.gameObject.SetActive(true);
            else if (child.gameObject.activeSelf)
                child.gameObject.SetActive(false);
        }
    }
}
