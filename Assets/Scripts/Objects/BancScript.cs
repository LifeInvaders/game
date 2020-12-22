using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BancScript : MonoBehaviour
{
    public int nbPlayers;

    // private List<Player> _playerList;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("new player");
            // _playerList.Add(other.gameObject.);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("remove player");
        }
    }
}
