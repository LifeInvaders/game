using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private ParticleSystem cannon;
    [SerializeField] private GameObject player;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PhotonNetwork.Instantiate("CustomCharacter",gameObject.transform.position,gameObject.transform.rotation);
    }
    private void Update()
    {
        
    }

    public override void OnLeftRoom() => SceneManager.LoadScene("MainMenu");
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        cannon.Play();
    }
}
