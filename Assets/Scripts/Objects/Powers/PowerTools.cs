using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Objects.Powers
{
    public abstract class PowerTools : MonoBehaviour
    {
        protected float TimeBeforeUse;
        protected int _time;
        protected Random _random;
        public bool gracePeriod;
        
        
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

        void Awake()
        {
            if (!gameObject.GetPhotonView().IsMine) enabled = false;
        }

        public void OnPower()
        {
            if ( !gracePeriod && enabled && TimeBeforeUse == 0 && IsValid())
            {
                TimeBeforeUse = _time;
                Action();
            }
        }
    }
    
}