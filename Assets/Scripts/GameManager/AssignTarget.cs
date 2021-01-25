using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class AssignTarget : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        charList = GameObject.FindGameObjectsWithTag("Player");  //Update char list
    }

    [PunRPC]
    private void ChangeTarget(Photon.Realtime.Player hunter, Photon.Realtime.Player target)
    {
        if (PhotonNetwork.LocalPlayer.Equals(hunter)) //Since RPC goes to everyone, we filter on arrival
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
        Debug.Log("You don't have a target!!!"); //Error case
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
            photonView.RPC("ChangeTarget",RpcTarget.AllViaServer,player, target);
            //RPC to everyone, let clients detect target
        }
    }
}