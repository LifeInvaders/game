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
    
    // Start is called before the first frame update
    void Awake()
    {
        _currentEndTime = Double.PositiveInfinity;
        timerText.gameObject.SetActive(true);
        PhotonNetwork.AddCallbackTarget(this);
        ChangeTimer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(1,null,raiseEventOptions,SendOptions.SendReliable);
        }
        if (PhotonNetwork.Time >= _currentEndTime)
            TimerEnded();
        UpdateTimer();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        propertiesThatChanged.TryGetValue("endTime",out var endTime);
        _currentEndTime = Convert.ToDouble(endTime);
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
        //TODO: Might want to reset target to null
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
    
    void UpdateTimer()
    {
        double diff = _currentEndTime - PhotonNetwork.Time;
        if (diff < 0) diff = 0;
        timerText.text = ((int)(diff / 60)).ToString("00") + ':' + ((int)(diff % 60)).ToString("00");
    }
}
