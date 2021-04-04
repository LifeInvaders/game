using System;
using UnityEngine;

namespace Objects.Powers
{
    public abstract class PowerTools : MonoBehaviour
    {
        protected float TimeBeforeUse;
        protected int _time;

        /*public int GetTime()
        {
            return _time;
        }
    
        public int GetTimeLeft()
        {
            return TimeBeforeUse;
        }*/

        protected abstract void Action();

        public void Update()
        {
            if (Input.GetButtonDown("Action") && TimeBeforeUse == 0)
            {
                TimeBeforeUse = _time;
                Action();
            }
            else if (TimeBeforeUse > 0)
            {
                TimeBeforeUse -= Time.deltaTime;
            }
        }
    }
    
}