using Photon.Pun;
﻿using System;
using System.Collections;
using HUD;
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
        public bool gracePeriod = true;
        
        
        private PowerHud _powerHud;

        private void Start()
        {
            SetValues();
            _powerHud = GetComponentInChildren<PowerHud>();
            _powerHud.SetIcon(this);
            if (TimeBeforeUse > 0)
            {
                _powerHud.SetTime(TimeBeforeUse);
            }
        }

        protected abstract void SetValues();

        /// <summary>
        /// Check if the player need to press for N seconds the key to use the power
        /// </summary>
        /// <returns></returns>
        protected bool IsShortAction = true;

        protected float TimeToStayOnTheButton = 0;
        private Coroutine _coroutine;
        private GameObject _instanceLoadingSelector;
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
            {
                TimeBeforeUse = 0;
                _powerHud.HideTimer();
            }
        }

        void Awake()
        {
            if (!gameObject.GetPhotonView().IsMine) enabled = false;
        }

        public void OnPower(InputValue inputValue)
        {
            if (gracePeriod || !enabled || TimeBeforeUse != 0 || !IsValid()) return;
            if (IsShortAction && inputValue.isPressed) ActivatePower();
            else
            {
                bool coroutineIsNull = _coroutine == null;
                if (inputValue.isPressed && coroutineIsNull)
                    _coroutine = StartCoroutine(WaitButtonPressed());
                else if (!coroutineIsNull && !inputValue.isPressed)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                    Destroy(_instanceLoadingSelector);
                }
            }
        }
        
        private IEnumerator WaitButtonPressed()
        {
            var target = GetComponent<SelectedTarget>().GetTarget();
            var InstanceLoadingSelector = Instantiate(loadingSelector, target.transform.position + Vector3.up * 2.4f,
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
            ActivatePower();
        }

        private void ActivatePower()
        {
            TimeBeforeUse = _time;
            _powerHud.SetTime(_time);
            Action();
        }
    }
}