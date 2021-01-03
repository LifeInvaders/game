using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            player.SetRotateBool(false);
            player.SetMoveBool(false);
            other.gameObject.GetComponent<Animator>().SetBool("Echelle",true);
            other.gameObject.transform.eulerAngles = transform.eulerAngles;
            if (gameObject.transform.transform.eulerAngles.x != 0)
            {
                // other.gameObject.GetComponent<CapsuleCollider>().center += Vector3.forward * 0.1f;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ExitLadder(other.gameObject);
            other.gameObject.transform.Translate(Vector3.forward *1.3f);
        }
    }

    private bool IsGrounded(GameObject player)
    {
        CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
        return Physics.Raycast(capsule.bounds.center, Vector3.down,capsule.bounds.extents.y+0.2f);
    }
    private void ExitLadder(GameObject playerGameObject)
    {
        PlayerControler player = playerGameObject.GetComponent<PlayerControler>();
        playerGameObject.GetComponent<Rigidbody>().useGravity = true;
        player.SetRotateBool(true);
        player.SetMoveBool(true);
        
        playerGameObject.GetComponent<Animator>().SetBool("Echelle",false);
        
        playerGameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           float input = Input.GetAxis("Vertical");
           if (input == 0)
               other.gameObject.GetComponent<Animator>().enabled = false;
           else if (input < 0 && IsGrounded(other.gameObject))
           {
               ExitLadder(other.gameObject);
               other.gameObject.transform.Translate(Vector3.back * 1.3f);
           }
           else
           {
               other.gameObject.GetComponent<Animator>().enabled = true;
               other.gameObject.transform.Translate(Vector3.up * (Time.deltaTime) * input);
           }
        }
            
    }
}
