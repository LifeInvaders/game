using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalking : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Animator>().SetBool("talking",true);
            // other.gameObject.GetComponent<PlayerControler>().SetRotateBool(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Animator>().SetBool("talking",false);
            // other.gameObject.GetComponent<PlayerControler>().SetRotateBool(true);
        }
    }
}
