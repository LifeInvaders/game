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
        public RaycastHit raycastHit;
        [SerializeField] private Camera _camera;

        private Outline _outlinecam;
        private GameObject _target;

        private bool _isOutlinecamNotNull;
        private SelectedTarget _selectedTarget;

        private bool _aiming;
        private bool _selected;

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
            if (_target != null && _target.name != raycastHit.transform.gameObject.name)
            {
                // Not the same player
                _outlinecam.enabled = false;
                _target = raycastHit.transform.gameObject;
                _outlinecam = playerOutline;
                _outlinecam.enabled = true;
            }
            else
            {
                _outlinecam = playerOutline;
                _target = raycastHit.transform.gameObject;
                _outlinecam.enabled = true;
            }
        }

        private void RemoveCamTarget()
        {
            _outlinecam.enabled = false;
            _outlinecam = null;
            _target = null;
        }

        private bool IsCharacter(GameObject gameObject) =>
            gameObject.CompareTag("NPC") || gameObject.CompareTag("Player");

        private void Update()
        {
            Debug.Log($"aiming : {_aiming} | {_selectedTarget.IsTarget()}");
            if (!_aiming)
            {
                if (_target != null && !_selectedTarget.IsSelectedTarget(_target))
                    RemoveCamTarget();
            }
            else
            {
                // bool tata = Physics.Raycast(_camera.transform.position,
                //     _camera.transform.TransformDirection(Vector3.forward),
                //     out raycastHit, 30f);
                // bool test = tata && IsCharacter(raycastHit.transform.gameObject);
          
                if (Physics.Raycast(_camera.transform.position,
                    _camera.transform.TransformDirection(Vector3.forward),
                    out raycastHit, 30f) && IsCharacter(raycastHit.transform.gameObject))
                {
                    if (!_selectedTarget.IsTarget())
                    {
                        Outlining(raycastHit.transform.Find("Character").GetComponent<Outline>());
                    }

                    if (_selected)
                    {
                        Debug.Log($"select {_target.name}");
                        _selectedTarget.UpdateSelectedTarget(_target, _outlinecam);
                        _selected = false;
                    }
                }
                else if (_target != null && !_selectedTarget.IsSelectedTarget(_target))
                    RemoveCamTarget();
            }
        }

        public void OnAim(InputValue value)
        {
            _aiming = !_aiming;
        }

        public void OnSelect(InputValue value)
        {
            _selected = value.isPressed && _aiming;
        }
    }
}