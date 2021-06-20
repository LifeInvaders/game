using Photon.Pun;
﻿using System;
using System.Collections;
using TargetSystem;
using UnityEngine;
using UnityEngine.InputSystem;
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
        /// Check if the player need to press for N seconds the key to use the power
        /// </summary>
        /// <returns></returns>
        protected bool IsShortAction = true;

        protected float TimeToStayOnTheButton = 0;
        private Coroutine _coroutine;
        private GameObject InstanceLoadingSelector;
        [SerializeField] private GameObject loadingSelector;

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
        private IEnumerator WaitButtonPressed()
        {
            if ( !gracePeriod && enabled && TimeBeforeUse == 0 && IsValid())
            var target = GetComponent<SelectedTarget>().GetTarget();
            InstanceLoadingSelector = Instantiate(loadingSelector, target.transform.position + Vector3.up * 2.4f,
                target.transform.rotation, target.transform);
            var holdUI = InstanceLoadingSelector.GetComponent<HoldUI>();
            holdUI.player = transform;
            holdUI.time = TimeToStayOnTheButton;
            InstanceLoadingSelector.GetComponent<Canvas>().worldCamera = Camera.current;
            float waitingTime = 0;
            while (waitingTime < TimeToStayOnTheButton)
            {
                waitingTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            TimeBeforeUse = _time;
            Action();
        }

        public void OnPower(InputValue inputValue)
        {
            if (!enabled || TimeBeforeUse != 0 || !IsValid()) return;
            
            if (IsShortAction && inputValue.isPressed)
            {
                TimeBeforeUse = _time;
                Action();
            }
            else
            {
                bool coroutineIsNull = _coroutine == null;
                if (inputValue.isPressed && coroutineIsNull)
                    _coroutine = StartCoroutine(WaitButtonPressed());
                else if (!coroutineIsNull && !inputValue.isPressed)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                    Destroy(InstanceLoadingSelector);
                }
            }
        }
    }
}