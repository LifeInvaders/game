using System;
using People.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tutorial
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private Tutorial tutorial;

        private bool _activated;


        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MyPlayer") && !_activated)
            {
                _activated = true;
                tutorial.arrivedToTrigger = true;
            }
        }

      
    }
}