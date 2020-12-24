using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class RandomChar : MonoBehaviour
    {
        private void Awake()
        {
            RandomCharacter();
        }

        public string randomID = "0M1A";
        public void RandomCharacter()
        {
            Transform[] transformList = GetComponentsInChildren<Transform>();
            foreach (Transform child in transformList)
            {
                if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
            }
            Transform randModel = transformList[Random.Range(0, transformList.Length)];
            randModel.gameObject.SetActive(true);
            ref string rID = ref randomID;
            rID = randModel.name;
        }
    }
}