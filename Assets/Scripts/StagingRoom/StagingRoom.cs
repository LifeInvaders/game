using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace StagingRoom
{
    public class StagingRoom : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private VideoPlayer videoPlayer;
        void Start()
        {
            gameObject.GetComponent<SaveDatabase>().Load();
            SetFPS();
            StartCoroutine(VideoCoroutine());
        }

        private IEnumerator VideoCoroutine()
        {
            yield return new WaitForSeconds(2);
            yield return new WaitUntil(() => videoPlayer.isPaused || Input.anyKey);
            SceneManager.LoadScene("MainMenu");
        }

        private void SetFPS()
        {
            QualitySettings.vSyncCount = 0; // VSync must be disabled
            Application.targetFrameRate = PlayerDatabase.Instance.fpsSettings;
        }
    }
}