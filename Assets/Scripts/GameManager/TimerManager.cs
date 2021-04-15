using System;
using GameManager;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.UI;



public class TimerManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private double[] timerPhase;
    private int _index;
    private double _currentEndTime;
    [SerializeField] private AssignTarget targetSystem;
    [SerializeField] private Text timerText;
    [SerializeField] private InGameStats igs;
    
    // Start is called before the first frame update
    void Awake()
    {
        _currentEndTime = Double.PositiveInfinity;
        PhotonNetwork.AddCallbackTarget(this);
        ChangeTimer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventManager.KilledEventCode,null,raiseEventOptions,SendOptions.SendReliable);
        }
        if (PhotonNetwork.Time >= _currentEndTime)
            TimerEnded();
        UpdateTimer();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        propertiesThatChanged.TryGetValue("endTime",out var endTime);
        _currentEndTime = Convert.ToDouble(endTime);
        if (!timerText.gameObject.activeInHierarchy) timerText.gameObject.SetActive(true);
    }

    void TimerEnded()
    {
        if (_index == 0)
            StartHunt();
        else EndRound();
    }

    void StartHunt()
    {
        _index = 1;
        ChangeTimer();
        if (PhotonNetwork.IsMasterClient)
            targetSystem.TargetAssigner();
        //TODO: Enable power usage
        //TODO: Enable hunt-related UI elements
    }

    void EndRound()
    {
        _index = 0;
        ChangeTimer();
        igs.target = null;
        //TODO: Disable power usage
        //TODO: Disable hunt-related UI elements
    }

    void ChangeTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable timer = new Hashtable {{"endTime", PhotonNetwork.Time + timerPhase[_index]}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(timer);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code != EventManager.KilledEventCode || _index >= timerPhase.Length - 1) return;
        _index += 1;
        if (_currentEndTime < PhotonNetwork.Time + timerPhase[_index])
            ChangeTimer();
    }

    [PunRPC]
    public void EnableTimer() => timerText.gameObject.SetActive(true);

    void UpdateTimer()
    {
        double diff = _currentEndTime - PhotonNetwork.Time;
        if (diff < 0) diff = 0;
        timerText.text = ((int)(diff / 60)).ToString("00") + ':' + ((int)(diff % 60)).ToString("00");
    }
}
