using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class StopTime : MonoBehaviour
    {
        private bool _pause = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) 
                Time.timeScale = _pause ? 0 : 1;
            _pause = !_pause;
        }
    }
}