using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Doors : MonoBehaviour
{
    private Transform _leftDoor;

    private Transform _rightDoor;

    private NavMeshObstacle _obstacle;

    private bool _opened = true;
    // Start is called before the first frame update
    private void Start()
    {
        _leftDoor = transform.Find("left door");
        _rightDoor = transform.Find("right door");

        // _obstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_opened && other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerControler>().Running())
        {
            _opened = false;
            Debug.Log($"{_leftDoor.eulerAngles} {_rightDoor.eulerAngles}");
            _leftDoor.eulerAngles -= Vector3.up * -90;
            _rightDoor.eulerAngles -= Vector3.up * 90;
            // _obstacle.carving = true;
            StartCoroutine(ExampleCoroutine());
        }
    }
    IEnumerator ExampleCoroutine()
    {
        
        yield return new WaitForSeconds(5);
        
        _opened = true;
        // Debug.Log($"{left_door.eulerAngles} {right_door.eulerAngles}");
        _leftDoor.eulerAngles += Vector3.up * (-90);
        _rightDoor.eulerAngles += Vector3.up * 90;
        // _obstacle.carving = false;
        // Debug.Log($"{left_door.eulerAngles} {right_door.eulerAngles}");
    }
}
