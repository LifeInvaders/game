using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ListPlayers : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] public Text printPlayer;
    [SerializeField] public Canvas playerBoard;

    private List<Player> _players;
    
    void Start()
    {
        
        foreach (var player in PhotonNetwork.PlayerList)
        {
            _players.Add(player);
        }

        foreach (var player in _players)
        {
            printPlayer.text += player.NickName + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!_players.Contains(player))
                {
                    _players.Add(player);    
                }
            }

            foreach (var player in _players)
            {
                if (!printPlayer.text.Contains(player.NickName))
                {
                    printPlayer.text += player.NickName + "\n";
                }
            }
            playerBoard.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerBoard.enabled = false;
        }
    }
}
