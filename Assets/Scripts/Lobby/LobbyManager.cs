using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private ParticleSystem cannon;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PhotonNetwork.Instantiate("CustomCharacter",gameObject.transform.position,gameObject.transform.rotation);
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
