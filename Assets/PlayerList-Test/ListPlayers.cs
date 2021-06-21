using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ListPlayers : MonoBehaviourPunCallbacks
{
    
    // Start is called before the first frame update
    [SerializeField] public TextMeshProUGUI printPlayer;
    [SerializeField] public Canvas playerBoard;

    private List<Photon.Realtime.Player> _players;

    void Start()
    {
        _players = new List<Photon.Realtime.Player>();
        playerBoard.enabled = (false);
        UpdateList();
    }


    void UpdateList()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);  
                printPlayer.text += player.NickName + "\n";
            }
            
        }
        printPlayer.text += "UpdateList" + "\n";
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            UpdateList();
            playerBoard.enabled = (true);
            printPlayer.text += "Update" + "\n";
            
        }

        if (Keyboard.current.tabKey.wasReleasedThisFrame)
        {
            playerBoard.enabled = (false);
        }
    }
}


