using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class VoidTutorial : MonoBehaviour
    {
        [SerializeField] private Tutorial tutorial;

        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MyPlayer")) 
                StartCoroutine(WaitFor());
        }

        private IEnumerator WaitFor()
        {
            tutorial.TeleportToLevel();
            tutorial.SetTutorial("Jeune matelos, ne tombe pas dans l'eau, tu vas te noyer !!!");
            while (!tutorial.GetDiscussion().HasExited)
                yield return new WaitForEndOfFrame();
        }
    }
}