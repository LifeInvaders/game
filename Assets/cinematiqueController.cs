using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using People.Player;
using TargetSystem;
using UnityEngine;

public class cinematiqueController : MonoBehaviour
{
    // Start is called before the first frame update
    public void SwitchToPlayer(GameObject player)
    {
        player.GetComponent<PlayerControler>().enabled = true;
        player.GetComponent<CameraControler>().enabled = true;
        player.GetComponent<CastTarget>().enabled = true;
        GetComponentInChildren<CinemachineVirtualCamera>().Priority = 0;
    }
}