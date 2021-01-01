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
            
            other.gameObject.GetComponent<Animator>().SetBool("Echelle",true);
            other.gameObject.transform.eulerAngles = transform.eulerAngles;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            player.SetRotateBool(true);
            other.gameObject.GetComponent<Animator>().SetBool("Echelle",false);
              
            // transform.Translate(Vector3.up*0.9f+Vector3.forward);
            
            other.gameObject.transform.Translate(Vector3.forward *1.3f);
            other.gameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);  
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.transform.Translate( Vector3.up * (Time.deltaTime));
    }
}
