using System;
using System.Collections;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TargetSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace People.Player
{
    public class PhotonPlayerEvent : HumanEvent
    {
        private Animator _animator;
        private CastTarget _castTarget;
        private KillTarget _killTarget;
        private PlayerControler _playerControler;

        [SerializeField] private Volume _volume;


        private void Start()
        {
            _playerControler = GetComponent<PlayerControler>();
            _killTarget = GetComponent<KillTarget>();
            _castTarget = GetComponent<CastTarget>();
            _animator = GetComponent<Animator>();
        }

        public override void Death()
        {
            throw new NotImplementedException();
        }

        public override void TriggeredBySmokeBomb(float endtime)
        {
            _playerControler.SetMoveBool(false);
            _castTarget.enabled = false;
            _castTarget.SetAiming(false, true);
            _killTarget.enabled = false;
            humanTask = HumanTasks.SmokeBomb;
            _animator.SetTrigger("smoked");
            StartCoroutine(WaitSmokeBomb(endtime));
        }

        public override void KilledByPoison()
        {
            throw new NotImplementedException();
        }

        public override void HarmedByKnife()
        {
            var value = _playerControler.GetWalkSpeed();
            _playerControler.SetWalkSpeed(1.2f);
            _playerControler.SetCanRun(false);
            _playerControler.CheckIfRunning();
            _animator.SetTrigger("injured");

            humanTask = HumanTasks.Bleeding;

            StartCoroutine(WaitKnife(value));
        }

        public override IEnumerator DeathByGun()
        {
            yield return new WaitForSeconds(8);
        }

        private IEnumerator WaitKnife(float value)
        {
            yield return new WaitForSeconds(8);

            _playerControler.SetCanRun(true);
            _playerControler.SetWalkSpeed(value);
            _playerControler.CheckIfRunning();
            _animator.SetTrigger("Default");
            humanTask = HumanTasks.Nothing;
        }

        private IEnumerator SpawnPoison()
        {
            throw new NotImplementedException();
        }

        IEnumerator WaitSmokeBomb(float endtime)
        {
            yield return new WaitForSeconds(endtime);
            _playerControler.SetMoveBool(true);
            _animator.SetTrigger("Default");
            _castTarget.enabled = true;
            _killTarget.enabled = true;
            humanTask = HumanTasks.Nothing;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                HarmedByKnife();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                KilledByPoison();
            }
        }
    }
}