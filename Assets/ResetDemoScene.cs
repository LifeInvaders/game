using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetDemoScene : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene("test3");
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        //     SceneManager.LoadScene("Scenes/PhotonScene");
    }
}
