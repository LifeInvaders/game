using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private ParticleSystem cannon;
    private float _currentTime = 180;
    [SerializeField]private Text timerText;
    [SerializeField] private Text notEnoughPlayers;
    private bool _startTimer;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PhotonNetwork.Instantiate("CustomCharacter",gameObject.transform.position,gameObject.transform.rotation);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PhotonNetwork.LeaveRoom();
        //if (currentTime == 0 && PhotonNetwork.LocalClient.IsMaster) load game map using PhotonLoad()
    }

    public override void OnLeftRoom() => SceneManager.LoadScene("Scenes/PhotonScene");

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (_startTimer && PhotonNetwork.CurrentRoom.MaxPlayers < PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            CancelInvoke();
            _currentTime = 180;
            _startTimer = false;
            UpdateUI();
            notEnoughPlayers.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        cannon.Play();
        if (!_startTimer && PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            _startTimer = true;
            InvokeRepeating("SubstractTime",0,1);
            notEnoughPlayers.gameObject.SetActive(false);
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && _currentTime > 30)
            _currentTime = 30;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            photonView.RPC("SetTimer", newPlayer, _currentTime,_startTimer,PhotonNetwork.Time);
    }

    [PunRPC]
    private void SetTimer(float value,bool state,double lag)
    {
        _currentTime = value - (float)(PhotonNetwork.Time - lag);
        _startTimer = state;
        UpdateUI();
        if (_startTimer) InvokeRepeating("SubstractTime",0,1);
    }

    private void SubstractTime()
    {
        _currentTime--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        timerText.text = ((int)(_currentTime / 60)).ToString("00") + ":" +
                         ((int)(_currentTime % 60)).ToString("00");
    }
}
