﻿using System;
using ExitGames.Client.Photon;
using Photon.Compression;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTimer : MonoBehaviourPunCallbacks
{
    private double _endTime;
    private bool _startTimer;
    public double halfTime = 180;
    public double fullTime = 30;
    [SerializeField]private Text timerText;
    [SerializeField] private Text notEnoughPlayers;
    [SerializeField] private Text playerCount;
    private void Awake()
    { 
        if (PhotonNetwork.IsMasterClient)
        {
            _endTime = PhotonNetwork.Time + halfTime;
            _startTimer = false;
            Hashtable customTimer = new Hashtable {{"endTimeLobby", _endTime},{"startTimerLobby",_startTimer}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(customTimer);
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("restart"))
            {
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    OnPlayerEnteredRoom(player);
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("endTimeLobby", out var time)) _endTime = Convert.ToDouble(time);
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("startTimerLobby", out var start)) _startTimer = (bool) start;
            UpdateTimer();
        }
    }
    private void Update()
    {
        if (_startTimer)
        {
            UpdateTimer();
            if (PhotonNetwork.Time >= _endTime && PhotonNetwork.IsMasterClient)
            {
                TimerEnd();
                enabled = false;
            }
        }
    }


    public void TimerEnd()
    {
        Debug.Log("Timer ended. Loading map...");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("Map");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient && _endTime > 0 && PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            Hashtable customTimer = new Hashtable {{"endTimeLobby", PhotonNetwork.Time + halfTime},{"startTimerLobby",false}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(customTimer);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!_startTimer && PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers / 2)
        {
            Hashtable customTimer = new Hashtable() {{"endTimeLobby", PhotonNetwork.Time + halfTime}, {"startTimerLobby", true}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(customTimer);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers &&
                 _endTime - PhotonNetwork.Time > fullTime)
        {
            Hashtable customTimer = new Hashtable() {{"endTimeLobby", PhotonNetwork.Time + fullTime}, {"startTimerLobby", true}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(customTimer);
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("Custom properties changed. Updating timer...");
        if (propertiesThatChanged.TryGetValue("endTimeLobby",out var time)) _endTime = Convert.ToDouble(time);
        if (propertiesThatChanged.TryGetValue("startTimerLobby", out var start)) _startTimer = (bool) start;
        UpdateUI();
    }

    private void UpdateUI()
    {
        notEnoughPlayers.gameObject.SetActive(!_startTimer);
        timerText.gameObject.SetActive(_startTimer);
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + '/' + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    private void UpdateTimer()
    {
        double diff = _endTime - PhotonNetwork.Time;
        if (diff < 0) diff = 0;
        timerText.text = ((int)(diff / 60)).ToString("00") + ':' + ((int)(diff % 60)).ToString("00");
    }
}
