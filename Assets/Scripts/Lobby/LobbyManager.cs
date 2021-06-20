﻿using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private ParticleSystem cannon;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            GenerateRoomSeed();
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("restart"))
        {
            Hashtable hash = new Hashtable {{"restart", false}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PhotonNetwork.Instantiate("CustomCharacter",gameObject.transform.position,gameObject.transform.rotation);
    }

    private void GenerateRoomSeed()
    {
        var _rand = new System.Random();
        Hashtable seed = new Hashtable {{"seed", _rand.Next()}};
        PhotonNetwork.CurrentRoom.SetCustomProperties(seed);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() => SceneManager.LoadScene("MainMenu");
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        cannon.Play();
    }
}
