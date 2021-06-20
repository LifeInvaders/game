﻿using System.Collections;
using Cinemachine;
using People;
using People.NPC;
using People.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.AI;

namespace TargetSystem
{
    public class KillTarget : MonoBehaviour
    {
        // Start is called before the first frame update
        private SelectedTarget _selectedTarget;
        private Animator _animator;

        private bool _killClick;
        private CastTarget _casttarget;

        [SerializeField] private GameObject[] finishers;
        public void SetFinisher(GameObject finisher) => finishers = new[] {finisher};
        


        void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
                enabled = false;
            _animator = GetComponent<Animator>();
            _selectedTarget = GetComponent<SelectedTarget>();
            _casttarget = GetComponent<CastTarget>();
        }

        /// <summary>
        /// Méthode appelée quand un joueur tue la cible verouillée 
        /// </summary>
        /// <param name="target"></param>
        void Kill(GameObject target)
        {
            target.GetComponent<HumanEvent>().Death();
            _casttarget.SetAiming(false);
            _casttarget.enabled = false;
            Debug.Log($"killed {target.name}");
            
            var g = Instantiate(finishers[Random.Range(0,finishers.Length)], transform.position, transform.rotation);

            var finisher = g.GetComponent<Finisher>();
            finisher.SetHumans(GetComponentInChildren<SkinnedMeshRenderer>(),
                target.GetComponentInChildren<SkinnedMeshRenderer>());
            finisher.player = gameObject;
            finisher.cinemachineBrain = GetComponentInChildren<CinemachineBrain>();
            
            
            
            
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<PlayerControler>().SetMoveBool(false);
            _selectedTarget.UpdateSelectedTarget(target, target.GetComponentInChildren<Outline>());

            // StartCoroutine(WaitForDeathAnim(target));
        }

        public void OnAttack(InputValue value)
        {
            if (value.isPressed)
            {
                GameObject target = _selectedTarget.GetTarget();
                // Si le joueur est à moins d'un mètre et demi.
                if (target != null && Vector3.Distance(transform.position, target.transform.position) < 1.5f)
                    Kill(target);
            }
        }
    }
}