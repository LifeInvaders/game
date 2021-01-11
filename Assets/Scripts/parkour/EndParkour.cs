using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParkour : MonoBehaviour
{
    public Transform startPosition;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player finished the parkour at {Time.time}");
            
            PlayerControler player = other.GetComponent<PlayerControler>();
            
            player.SetJumpSpeed(5);
            player.SetWalkSpeed(3);
            player.SetRunSpeed(6);
            other.GetComponent<Transform>().position = startPosition.position;
        }
    }
}
