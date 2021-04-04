using System;
using System.Collections;
using System.Collections.Generic;
using People.Player;
using UnityEngine;

public class StartParkour : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player started the parkour at {Time.time}");

            PlayerControler player = other.GetComponent<PlayerControler>();
            
            player.SetJumpSpeed(5.8f);
            player.SetWalkSpeed(4);
            player.SetRunSpeed(8);
        }
    }
}
