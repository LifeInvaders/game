using System;
using System.Collections.Generic;
using GameManager;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Objects.Powers;
using People;
using Photon.Realtime;
using RadarSystem;
using TargetSystem;
using UnityEngine.UI;



public class TimerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private double[] timerPhase;
    private int _index;
    private double _currentEndTime;
    [SerializeField] private AssignTarget targetSystem;
    [SerializeField] private Text timerText;
    [SerializeField] private InGameStats igs;
    public bool ended = true;


    public void StartGame() => ChangeTimer();

    void Update()
    {
        if (!ended && PhotonNetwork.Time >= _currentEndTime)
        {
            ended = true;
            TimerEnded();
        }
        UpdateTimer();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (!propertiesThatChanged.TryGetValue("endTime", out var endTime)) return;
        Debug.Log("Changing timer");
        _currentEndTime = Convert.ToDouble(endTime);
        if (!timerText.gameObject.activeInHierarchy) timerText.gameObject.SetActive(true);
        if (ended) ended = false;
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
        igs.localPlayer.GetComponent<CastTarget>().enabled = true;
        igs.localPlayer.GetComponent<PowerTools>().gracePeriod = false;
        if (PhotonNetwork.IsMasterClient) targetSystem.TargetAssigner();
    }

    void EndRound()
    {
        _index = 0;
        ChangeTimer();
        if (PhotonNetwork.IsMasterClient) 
            EventManager.RaiseEndRoundEvent();
        igs.localPlayer.GetComponent<CastTarget>().enabled = false;
        igs.localPlayer.GetComponent<PowerTools>().gracePeriod = true;
        igs.localPlayer.GetComponentInChildren<RandomSkin>(true).transform.parent.gameObject.SetActive(false);
        var targetSelect = igs.localPlayer.GetComponent<SelectedTarget>();
        targetSelect.UpdateSelectedTarget(null,null);
    }

    void ChangeTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Changing timer");
            Hashtable timer = new Hashtable {{"endTime", PhotonNetwork.Time + timerPhase[_index]}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(timer);
        }
    }

    public void AccTimer()
    {
        if (_index == timerPhase.Length - 1) return;
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
