using System;
using UnityEngine;
using Random = System.Random;

namespace Objects.Powers
{
    public abstract class PowerTools : MonoBehaviour
    {
        protected float TimeBeforeUse;
        protected int _time;
        protected Random _random;
        
        
        /// <summary>
        /// Use this function if the player needs to validate certain parameters in order to use the Power
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsValid();
        /// <summary>
        ///  Method called when the power is used
        /// </summary>
        protected abstract void Action();

        public void FixedUpdate()
        {
            if (TimeBeforeUse > 0)
            {
                TimeBeforeUse -= Time.deltaTime;
            }
            else if (TimeBeforeUse < 0)
                TimeBeforeUse = 0;
        }

        public void OnPower()
        {
            if (enabled &&TimeBeforeUse == 0 && IsValid())
            {
                TimeBeforeUse = _time;
                Action();
            }
        }
    }
    
}