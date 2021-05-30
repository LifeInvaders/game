using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private int numberOfNpc;
    private Transform[] _points;
    private System.Random _random;
    [SerializeField] private Transform pointsParent;
    [NonSerialized] public int dead;
    [SerializeField] private double timeSync = 15;

    void Start()
    {
        _points = pointsParent.GetComponentsInChildren<Transform>();
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 1; i <= numberOfNpc; i++)
            {
                PhotonNetwork.InstantiateRoomObject("PhotonNPC", _points[i % _points.Length].position, Quaternion.identity);
            }
            SetTimerForChainLoading();
        }
    }

    void SetTimerForChainLoading()
    {
        var timer = new Hashtable {{"npcDone", PhotonNetwork.Time + 20}};
        PhotonNetwork.CurrentRoom.SetCustomProperties(timer);
    }

    public void StartRespawnCoroutine() => StartCoroutine(Respawn());

    IEnumerator Respawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(5);
            for (int i = 0; i < dead; i++)
            {
                PhotonNetwork.InstantiateRoomObject("PhotonNPC", _points[i % _points.Length].position, Quaternion.identity);
            }
            dead = 0;
        }
    }
}
