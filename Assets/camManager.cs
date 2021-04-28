using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class camManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private CinemachineVirtualCamera[] cameras;
    private int _index = 0;


    // Update is called once per frame

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(_index);
            cameras[_index].Priority = 10;
            cameras[(_index + 1) % cameras.Length].Priority = 20;
            _index = (_index + 1) % cameras.Length;
        }
    }
}