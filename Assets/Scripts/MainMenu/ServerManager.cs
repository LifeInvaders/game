using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SRandom = System.Random;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField nickName;
    [SerializeField] private string quickJoinMap;

    private static SRandom random = new SRandom();

    private void Start()
    {
        PhotonNetwork.NickName = Player.PlayerDatabase.Instance.Nickname;
        nickName.text = Player.PlayerDatabase.Instance.Nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        roomName = PhotonNetwork.NickName + "'s Room";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnCreatedRoom() => PhotonNetwork.LoadLevel(quickJoinMap);

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
            { MaxPlayers = maxPlayers, IsVisible = true };
        var randomRoomName = RandomString(6);
        if (!PhotonNetwork.CreateRoom(randomRoomName, quickOptions))
        {
            for (int roomNumber = 2;
                roomNumber < 10 && !PhotonNetwork.CreateRoom(randomRoomName + roomNumber, quickOptions);
                roomNumber++)
            {
            }
        }
    }


    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [SerializeField] private int chooseMap;
    [SerializeField] private bool isPrivate;
    [SerializeField] private byte maxPlayers = 8;
    [SerializeField] private string roomName;


    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
            { MaxPlayers = maxPlayers, IsVisible = isPrivate };
        roomOptions.CustomRoomProperties.Add("Map", chooseMap);
        roomOptions.CustomRoomPropertiesForLobby = new[] { "Map" };
        if (!PhotonNetwork.CreateRoom(roomName, roomOptions))
            Debug.Log("Room creation failed: Room already exists with that name!");
    }
}