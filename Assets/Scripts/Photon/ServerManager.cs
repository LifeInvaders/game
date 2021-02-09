using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerManager : MonoBehaviourPunCallbacks
{

    [SerializeField]private string nickName;
    private void Start()
    {
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.ConnectUsingSettings())
            Debug.Log("Unable to connect to servers. Try again later.");
        Debug.Log(PhotonNetwork.IsConnected);
        roomName = PhotonNetwork.NickName + "'s Room";
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    
    public void QuickJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    

    public void TryJoin(string roomName)
    {
        if (!PhotonNetwork.JoinRoom(roomName))
        {
            Debug.Log("Unable to join room \"" + roomName + "\". Incorrect name?");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions quickOptions = new RoomOptions
                {MaxPlayers = 8, IsVisible = true};
            if (!PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room",quickOptions))
            {
                for (int roomNumber = 2; roomNumber < 10 && !PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room" + (char) ('0' + roomNumber), quickOptions); roomNumber++){}
            }
    }

    [SerializeField]private int chooseMap = 0;
    [SerializeField]private bool isPrivate = false;
    [SerializeField]private byte maxPlayers = 8;
    [SerializeField] private string roomName;


    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
            {MaxPlayers = maxPlayers, IsVisible = isPrivate};
        roomOptions.CustomRoomProperties.Add("map",chooseMap);
        roomOptions.CustomRoomPropertiesForLobby = new[] {"map"};
        if (!PhotonNetwork.CreateRoom(roomName, roomOptions))
        {
            Debug.Log("Room creation failed: Room already exists with that name!");
        }
    }
}
