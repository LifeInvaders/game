using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetSystem
{
    public class CastTarget : MonoBehaviour
    {
        public RaycastHit raycastHit;
        [SerializeField] private Camera _camera;
        private Outline _outlinetarget;

        private Outline _outlinecam;
        private GameObject _target;

        private bool _isOutlinecamNotNull;

        private void Update()
        {

            if (Input.GetButton("Fire2"))
            {
                if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward),
                    out raycastHit,30f) && (raycastHit.transform.gameObject.CompareTag("Player") ||
                                        raycastHit.transform.gameObject.CompareTag("NPC")))// && Vector3.Distance(raycastHit.transform.position,transform.position) < 12)
                {
                    Outline playerOutline = raycastHit.transform.Find("Character").GetComponent<Outline>();


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
                else if (_target != null)
                {
                    RemoveCamTarget();
                }
            }
            else if (_target != null)
            {
                RemoveCamTarget();
            }
        }

        private void RemoveCamTarget()
        {
            _outlinecam.enabled = false;
            _outlinecam = null;
            _target = null;
        }
    }
}