using UnityEngine;
using UnityEngine.Events;

namespace MainMenu
{
    public class WaitForKey : MonoBehaviour
    {
        public UnityEvent onAnyKey;
        void Update()
        {
            if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
            {
                onAnyKey.Invoke();
            }
        }
    }
}
