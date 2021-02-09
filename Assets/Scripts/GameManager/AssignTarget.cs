using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class AssignTarget : MonoBehaviourPunCallbacks
{

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            TargetAssigner();
    }
    [PunRPC]
    private void ChangeTarget(Photon.Realtime.Player target)
    { 
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))  //Test every Player character for correct target
        {
            if (player.GetPhotonView().Owner.Equals(target))  //Test if character belongs to target
            {
                gameObject.GetComponent<InGameStats>().target = player;  //Set target in InGameStats
                //The below message should be a UI element!!!
                Debug.Log("Your target is: " + player.GetPhotonView().Owner.NickName);
                return;
            }
        }
    }
    
    private Random _random = new Random();

    private void TargetAssigner()
    {
        var targetList = new Dictionary<Photon.Realtime.Player, Photon.Realtime.Player>();
        foreach(var player in PhotonNetwork.PlayerList)
        {
            //This is a filter for the valid targets
            var targetFilter = PhotonNetwork.PlayerList.Where(i => !targetList.ContainsValue(i) //Excludes already assigned targets
                                                                   && i.Equals(player)                     //Excludes targeting itself
                                                                   && (!targetList.ContainsKey(i) ||      //Exclude hunter's hunter (no 1v1)
                                                                       !targetList[i].Equals(player))).ToArray();
            var target = targetFilter[_random.Next(targetFilter.Length)]; //Pick a random player in the filtered list
            targetList.Add(player, target);  //Add the hunter/target combo to the dictionary
            photonView.RPC("ChangeTarget",player,target);
            //RPC to everyone, let clients detect target
        }
    }
}