using Discord_RPC;
using ExitGames.Client.Photon;
using Photon.Pun;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private ParticleSystem cannon;
    [SerializeField] private TextMeshProUGUI codeName;

    private void Start()
    {
        var player = PhotonNetwork.Instantiate("CustomCharacter", gameObject.transform.position,
            gameObject.transform.rotation);
        if (PhotonNetwork.IsMasterClient)
            GenerateRoomSeed();
        Hashtable winAnim = new Hashtable() { { "winAnim", PlayerDatabase.Instance.SelectedEmote } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(winAnim);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        codeName.text = $"Code : {PhotonNetwork.CurrentRoom.Name}";

        PresenceManager.UpdatePresence(detail: "Dans le lobby", size: PhotonNetwork.CurrentRoom.PlayerCount,
            max: PhotonNetwork.CurrentRoom.MaxPlayers, partyId: PhotonNetwork.CurrentRoom.Name,join:PhotonNetwork.CurrentRoom.Name +"0");
        
        
    }

    private void GenerateRoomSeed()
    {
        var _rand = new System.Random();
        Hashtable seed = new Hashtable { { "seed", _rand.Next() } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(seed);
    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape)) PhotonNetwork.LeaveRoom();
    // }

    public override void OnLeftRoom() => SceneManager.LoadScene("MainMenu");

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        cannon.Play();
        PresenceManager.UpdateParty(PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PresenceManager.UpdateParty(PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }
}