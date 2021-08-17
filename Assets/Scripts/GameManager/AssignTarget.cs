using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using People;
using Photon.Pun;
using RadarSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class AssignTarget : MonoBehaviourPunCallbacks
{
    [SerializeField] private InGameStats igs;
    private Random _random = new Random();
    private Radar _radar;
    [SerializeField] private Text targetName;
    private RandomSkin uiSkin; 
    


    [PunRPC]
    private void ChangeTarget(Photon.Realtime.Player target)
    {
        if (uiSkin == null) uiSkin = igs.localPlayer.GetComponentInChildren<RandomSkin>(true);
        igs.target = PhotonView.Find((int) target.CustomProperties["viewID"]).gameObject;
        if (_radar == null) _radar = igs.localPlayer.GetComponentInChildren<Radar>(true);
        _radar.gameObject.SetActive(true);
        _radar.SetTarget(igs.target.transform);
        StartCoroutine(SetTargetText(target.NickName));
        var targetSkin = igs.target.GetComponent<PhotonSkin>().GetSkinNpc();
        uiSkin.SetSkinNPC(targetSkin.mesh,targetSkin.material);
        uiSkin.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator SetTargetText(string name)
    {
        targetName.text = "Your target is " + name + '.';
        targetName.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        targetName.gameObject.SetActive(false);
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
                Photon.Realtime.Player[] targetFilter;
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
                {
                    targetFilter = PhotonNetwork.PlayerList.Where(i =>
                        !targetList.ContainsValue(i) //Excludes already assigned targets
                        && !i.Equals(player) //Excludes targeting itself
                        && (!targetList.ContainsKey(i) || //Exclude hunter's hunter (no 1v1)
                            !targetList[i].Equals(player))).ToArray();
                }
                else
                {
                    targetFilter = PhotonNetwork.PlayerList.Where(i => 
                                   !i.Equals(player)).ToArray();
                }
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
            photonView.RPC(nameof(ChangeTarget), kvp.Key, kvp.Value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && PhotonNetwork.LocalPlayer.NickName.Equals("TestClient"))
            TargetAssigner();
    }
}