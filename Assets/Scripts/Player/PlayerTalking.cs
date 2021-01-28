using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalking : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Animator>()
                .SetBool("talking", Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0);
        }
    }

}
