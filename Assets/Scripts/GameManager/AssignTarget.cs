using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using RadarSystem;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class AssignTarget : MonoBehaviourPunCallbacks
{
    [SerializeField] private InGameStats igs;
    private Random _random = new Random();
    private Radar _radar;


    [PunRPC]
    private void ChangeTarget(Photon.Realtime.Player target)    
    {
        igs.target = PhotonView.Find((int) target.CustomProperties["viewID"]).gameObject;
        if (_radar == null) _radar = igs.localPlayer.GetComponentInChildren<Radar>();
        _radar.gameObject.SetActive(true);
        _radar.SetTarget(igs.target.transform);
    }

    public void KilledTarget()
    {
        igs.target = null;
        _radar.SetTarget(null);
        _radar.gameObject.SetActive(false);
    }

    public void TargetAssigner()
    {
        var targetList = new Dictionary<Photon.Realtime.Player, Photon.Realtime.Player>();
        for (int tries = 0;targetList.Count == 0 && tries < 20;tries++)
        {
            Debug.Log("TargetAssigner: Attempt number " + tries);
            foreach (var player in PhotonNetwork.PlayerList)
            {
                //This is a filter for the valid targets
                var targetFilter = PhotonNetwork.PlayerList.Where(i =>
                    !targetList.ContainsValue(i) //Excludes already assigned targets
                    && !i.Equals(player) //Excludes targeting itself
                    && (!targetList.ContainsKey(i) || //Exclude hunter's hunter (no 1v1)
                        !targetList[i].Equals(player))).ToArray();
                if (targetFilter.Length == 0)
                {
                    targetList = new Dictionary<Photon.Realtime.Player, Photon.Realtime.Player>();
                    break;
                }
                var target =
                    targetFilter[_random.Next(targetFilter.Length)]; //Pick a random player in the filtered list
                targetList.Add(player, target); //Add the hunter/target combo to the dictionary
            }
        }
        foreach (KeyValuePair<Photon.Realtime.Player, Photon.Realtime.Player> kvp in targetList)
            photonView.RPC("ChangeTarget", kvp.Key, kvp.Value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && PhotonNetwork.LocalPlayer.NickName.Equals("TestClient"))
            TargetAssigner();
    }
}