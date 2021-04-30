using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using People.Player;
using TargetSystem;
using UnityEngine;

public class cinematiqueController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    public void SwitchToPlayer()
    {
        player.GetComponent<PlayerControler>().enabled = true;
        player.GetComponent<CameraControler>().enabled = true;
        player.GetComponent<CastTarget>().enabled = true;
        GetComponentInChildren<CinemachineVirtualCamera>().Priority = 0;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwitchToPlayer();
            gameObject.SetActive(false);
        }
    }
}