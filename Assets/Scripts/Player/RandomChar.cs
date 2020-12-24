using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Player
{
    public class RandomChar : MonoBehaviour
    {
        public void Update()
        {
            RandomCharacter();
        }
        
        private void RandomCharacter()
        {
            Transform[] transformList = GetComponentsInChildren<Transform>(includeInactive: true)
                .Where(child => child.gameObject != gameObject)
                .ToArray();
            foreach (Transform child in transformList)
            {
                if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
            }
            GameObject randModel = transformList[Random.Range(0, transformList.Length)].gameObject;
            randModel.SetActive(true);
            ref string rID = ref randomID;
            rID = randModel.name;
        }
        public string randomID = "0M1A";
    }
}