using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI

{
    public class SceneLoader : MonoBehaviour
    {
        public void SceneLoaderID(int sceneID)
        {
            string[] sceneList = {"PhotonScene", "CharacterCreation"};
            SceneManager.LoadScene(sceneList[sceneID]);
        }
    }
}

