using System;
using System.Collections;
using System.Collections.Generic;
using People.Player;
using UnityEngine;

public class PlayerTalking : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 axis = other.gameObject.GetComponent<PlayerControler>().GetAxis();
            other.gameObject.GetComponent<Animator>()
                .SetBool("talking", axis.Equals(Vector2.zero));
        }
    }

}
