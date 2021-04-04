using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField nickName;
    private void Start()
    {
        PhotonNetwork.NickName = Player.PlayerDatabase.Instance.Nickname;
        nickName.text = Player.PlayerDatabase.Instance.Nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        roomName = PhotonNetwork.NickName + "'s Room";
    }
    
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnCreatedRoom() => PhotonNetwork.LoadLevel("Lobby");

    public void SetNickname()
    {
        if (!string.IsNullOrEmpty(nickName.text))
        {
            PhotonNetwork.NickName = nickName.text;
            Player.PlayerDatabase.Instance.Nickname = nickName.text;
        }
    }

    public void QuickJoin() => PhotonNetwork.JoinRandomRoom();


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
                for (int roomNumber = 2; roomNumber < 10 && !PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room " + roomNumber, quickOptions); roomNumber++){}
            }
    }

    [SerializeField] private int chooseMap;
    [SerializeField] private bool isPrivate;
    [SerializeField] private byte maxPlayers = 8;
    [SerializeField] private string roomName;


    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
            {MaxPlayers = maxPlayers, IsVisible = isPrivate};
        roomOptions.CustomRoomProperties.Add("Map",chooseMap);
        roomOptions.CustomRoomPropertiesForLobby = new[] {"Map"};
        if (!PhotonNetwork.CreateRoom(roomName, roomOptions))
            Debug.Log("Room creation failed: Room already exists with that name!");
    }
}
