using System;
using System.Collections;
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
            SetSyncTime();
            for (int i = 1; i <= numberOfNpc; i++)
            {
                var npc =PhotonNetwork.InstantiateRoomObject("PhotonNPC", _points[i % _points.Length].position, Quaternion.identity);
            }
        }
    }

    public void StartRespawnCoroutine() => StartCoroutine(Respawn());

    IEnumerator Respawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetSyncTime();
            yield return new WaitForSeconds(5);
            for (int i = 0; i < dead; i++)
            {
                var npc =PhotonNetwork.InstantiateRoomObject("PhotonNPC", _points[i % _points.Length].position, Quaternion.identity);
            }
        }
    }
    
    void SetSyncTime()
    {
        Hashtable hashtable = new Hashtable {{"syncNpcStart", PhotonNetwork.Time + timeSync}};
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }
}
