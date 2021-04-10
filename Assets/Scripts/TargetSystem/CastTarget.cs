using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        /// <summary>
        /// Set Aiming bool
        /// </summary>
        /// <param name="value"></param>
        public void SetAiming(bool value)
        {
            _aiming = value;
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
                enabled = false;
            _selectedTarget = GetComponent<SelectedTarget>();

            _aiming = false;
            _selected = false;
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
            
            if (_aiming && Physics.Raycast(_camera.transform.position,
                _camera.transform.TransformDirection(Vector3.forward),
                out _raycastHit, 30f, 768)) // si on vise un personnage
            {
                if (_isTargetNull || (_selectedTarget.IsSelectedTarget(_raycastHit.transform.gameObject) && _raycastHit.transform.gameObject.GetInstanceID() != _target.GetInstanceID()))
                {
                    RemoveCamTarget();
                    Outlining(_raycastHit.transform.GetComponentInChildren<Outline>());
                }

                if (_selected)
                {
                    _selectedTarget.UpdateSelectedTarget(_target, _outlineCam);
                    _selected = false;
                    _aiming = false;
                }
            }
            else if (!_isTargetNull && !_selectedTarget.IsSelectedTarget(_target))
                RemoveCamTarget();
        }

        public void OnAim() => _aiming = !_aiming;

        public void OnSelect(InputValue value) => _selected = value.isPressed && _aiming;
    }
}