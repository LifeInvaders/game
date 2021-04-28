using System;
using System.Collections;
using System.Collections.Generic;
using People.Player;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace TargetSystem
{
    public class CastTarget : MonoBehaviour
    {
        private RaycastHit _raycastHit;
        [SerializeField] private Camera _camera;

        private Outline _outlineCam;
        public GameObject _target;

        private SelectedTarget _selectedTarget;

        public bool _aiming;
        private bool _selected;

        public bool _isTargetNull;

        public Volume vignette;
        [SerializeField] private Camera[] _cameras;

        private PlayerControler _playerControler;

        /// <summary>
        /// Set Aiming bool
        /// </summary>
        /// <param name="value"></param>
        public void SetAiming(bool value, bool playFade = false)
        {
            _aiming = value;
            if (playFade)
            {
                if (value)
                    StartCoroutine(FadeIn());
                else
                    StartCoroutine(FadeOut());
            }
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
                enabled = false;
            _selectedTarget = GetComponent<SelectedTarget>();
            _playerControler = GetComponent<PlayerControler>();
            _aiming = false;
            _selected = false;
        }

        IEnumerator FadeIn()
        {
            foreach (var cam in _cameras) cam.enabled = true;
            float time = 0;
            // _vignette.isGlobal = true;
            while (time <= 0.5f)
            {
                vignette.weight = time * 2;
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            vignette.weight = 1;
        }

        IEnumerator FadeOut()
        {
            float time = 0.5f;
            while (time >= 0)
            {
                vignette.weight = time * 2;
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            vignette.weight = 0;
            // _vignette.isGlobal = false;
            foreach (var cam in _cameras) cam.enabled = false;
        }


        /// <summary>
        /// Surligne le joueur et désactive la surbrillance de l'autre joueur s'il a changé
        /// </summary>
        /// <param name="playerOutline"></param>
        private void Outlining(Outline playerOutline)
        {
            _outlineCam = playerOutline;
            _target = _raycastHit.transform.gameObject;
            _outlineCam.enabled = true;
        }

        private void RemoveCamTarget()
        {
            if (!_isTargetNull)
            {
                _outlineCam.enabled = false;
                _outlineCam = null;
                _target = null;
            }
        }

        private void FixedUpdate()  
        {
            _isTargetNull = _target == null;
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.blue);
            
            if (_aiming && Physics.Raycast(_camera.transform.position,
                _camera.transform.TransformDirection(Vector3.forward),
                out _raycastHit, 30f,768) ) // si on vise un personnage 768
            {
                Debug.Log("player");
                if (_isTargetNull || (_selectedTarget.IsSelectedTarget(_raycastHit.transform.gameObject) &&
                                      _raycastHit.transform.gameObject.GetInstanceID() != _target.GetInstanceID()))
                {
                    RemoveCamTarget();
                    Outlining(_raycastHit.transform.GetComponentInChildren<Outline>());
                }

                if (_selected)
                {
                    _selectedTarget.UpdateSelectedTarget(_target, _outlineCam);
                    _selected = false;
                    SetAiming(false, true);
                }
            }
            else if (!_isTargetNull && !_selectedTarget.IsSelectedTarget(_target))
            {
                Debug.Log("remove end");
                RemoveCamTarget();
            }
        }

        public void OnAim()
        {
            SetAiming(!_aiming, true);
            if (_aiming && _selectedTarget.IsTarget())
                _selectedTarget.UpdateSelectedTarget(_target, _outlineCam);
        }

        public void OnSelect(InputValue value) => _selected = value.isPressed && _aiming && !_isTargetNull;
    }
}