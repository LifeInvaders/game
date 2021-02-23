using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI

{
    public class SceneLoader : MonoBehaviour
    {
        public void SceneLoaderName(string sceneName) => SceneManager.LoadScene(sceneName);
        public void SceneLoaderID(int sceneID) => SceneManager.LoadScene(sceneID);
    }
}

