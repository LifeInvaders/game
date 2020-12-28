using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Transform left_door;

    private Transform right_door;

    private bool opened = true;
    // Start is called before the first frame update
    private void Start()
    {
        left_door = transform.Find("left door");
        right_door = transform.Find("right door");
    }

    private void OnTriggerExit(Collider other)
    {
        if (opened && other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerControler>().Running())
        {
            opened = false;
            Debug.Log($"{left_door.eulerAngles} {right_door.eulerAngles}");
            left_door.eulerAngles -= Vector3.up * -90;
            right_door.eulerAngles -= Vector3.up * 90;
            StartCoroutine(ExampleCoroutine());
        }
    }
    IEnumerator ExampleCoroutine()
    {
        
        yield return new WaitForSeconds(5);
        
        opened = true;
        Debug.Log($"{left_door.eulerAngles} {right_door.eulerAngles}");
        left_door.eulerAngles += Vector3.up * (-90);
        right_door.eulerAngles += Vector3.up * 90;
        Debug.Log($"{left_door.eulerAngles} {right_door.eulerAngles}");
    }
}
