using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagingRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SaveDatabase>().Load();
        SetFPS();
        SceneManager.LoadScene("MainMenu");
    }

    private void SetFPS()
    {
        QualitySettings.vSyncCount = 0; // VSync must be disabled
        Application.targetFrameRate = PlayerDatabase.Instance.fpsSettings;
    }
}