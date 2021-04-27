using System;
using GameManager;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.UI;



public class TimerManager : MonoBehaviourPunCallbacks
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
        ChangeTimer();
    }

    void Update()
    {
        if (PhotonNetwork.Time >= _currentEndTime)
            TimerEnded();
        UpdateTimer();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (!propertiesThatChanged.TryGetValue("endTime", out var endTime)) return;
        _currentEndTime = Convert.ToDouble(endTime);
        timerText.gameObject.SetActive(true);
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
    }

    void EndRound()
    {
        _index = 0;
        ChangeTimer();
        igs.target = null;
        if (PhotonNetwork.IsMasterClient) 
            EventManager.RaiseEndRoundEvent();
        //TODO: Disable power usage
        //TODO: Disable hunt-related UI elements
        //TODO: Disable ability to kill
    }

    void ChangeTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable timer = new Hashtable {{"endTime", PhotonNetwork.Time + timerPhase[_index]}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(timer);
        }
    }

    public void AccTimer()
    {
        if (_index >= timerPhase.Length - 1) return;
        _index += 1;
        if (_currentEndTime > PhotonNetwork.Time + timerPhase[_index])
            ChangeTimer();
    }

    void UpdateTimer()
    {
        double diff = _currentEndTime - PhotonNetwork.Time;
        if (diff < 0) diff = 0;
        timerText.text = ((int)(diff / 60)).ToString("00") + ':' + ((int)(diff % 60)).ToString("00");
    }
}
