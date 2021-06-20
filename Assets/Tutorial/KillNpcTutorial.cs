using System;
using UnityEngine;

namespace Tutorial
{
    public class KillNpcTutorial : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        private void OnDestroy()
        {
            _tutorial.arrivedToTrigger = true;
        }
    }
}