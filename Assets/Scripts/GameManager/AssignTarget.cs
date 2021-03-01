using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class AssignTarget : MonoBehaviourPunCallbacks
{
    [SerializeField] private InGameStats igs;
    
    
    [PunRPC]
    private void ChangeTarget(Photon.Realtime.Player target)
    {
        foreach (TextMeshPro tmp in FindObjectsOfType<TextMeshPro>())
            tmp.color = Color.white;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))  //Test every Player character for correct target
        {
            if (player.GetPhotonView().Owner.Equals(target))  //Test if character belongs to target
            {
                Debug.Log("Player found!");
                igs.target = player;  //Set target in InGameStats
                Debug.Log("Target successfully set!");
                Debug.Log("Changing name color...");
                player.GetComponentInChildren<TextMeshPro>().color = Color.red; //For testing
                //Display a UI message!!!
                return;
            }
        }
    }
    
    private Random _random = new Random();

    private void TargetAssigner()
    {
        var targetList = new Dictionary<Photon.Realtime.Player, Photon.Realtime.Player>();
        while (targetList.Count == 0)
        {
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
        WriteDictToFile(targetList); //For testing
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && PhotonNetwork.LocalPlayer.NickName.Equals("TestClient"))
            TargetAssigner();
    }
    
    

    private void WriteDictToFile(Dictionary<Photon.Realtime.Player,Photon.Realtime.Player> dict)
    {
        string folderPath = Application.persistentDataPath + "/test/assign_target/";
        string timeStamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        StreamWriter file = File.CreateText(folderPath + "assign_target" + timeStamp + ".txt");
        file.WriteLine("Test " + timeStamp);
        file.WriteLine();
        foreach (KeyValuePair<Photon.Realtime.Player, Photon.Realtime.Player> kvp in dict)
            file.WriteLine(kvp.Key.NickName + "  ->  " + kvp.Value.NickName);
        file.Close();
    }
}