using System;
using People.Player;
using TargetSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class demoEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject finisher;

    // private Rigidbody rb;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // playerControler = GetComponent<PlayerControler>();
        // rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<KillTarget>().SetFinisher(finisher);
        }
    }

    // public void OnBoostDev()
    // {
    //     
    //     if (changeValue)
    //     {
    //         playerControler.SetWalkSpeed(6);
    //         playerControler.SetRunSpeed(12);
    //         playerControler.SetJumpSpeed(10);
    //     }
    //     else
    //     {
    //         playerControler.SetWalkSpeed(3);
    //         playerControler.SetRunSpeed(6);
    //         playerControler.SetJumpSpeed(5);
    //     }
    //     changeValue = !changeValue;
    // }
    //
    // public void OnGravityDev()
    // {
    //     rb.useGravity = !rb.useGravity;
    // }

    public void OnLeave()
    {
        SceneManager.LoadScene("Scenes/PhotonScene");
    }
    
}
