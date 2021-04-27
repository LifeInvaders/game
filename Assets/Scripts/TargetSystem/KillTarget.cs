using System.Collections;
using People;
using People.NPC;
using People.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.AI;
using Random = System.Random;

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
            Debug.Log($"killed {target.name}");
            var g = Instantiate(finishers[new Random().Next(finishers.Length)], transform.position, transform.rotation);
            
            
            g.GetComponent<Finisher>().SetHumans(GetComponentInChildren<SkinnedMeshRenderer>(),
                target.GetComponentInChildren<SkinnedMeshRenderer>());
            g.GetComponent<Finisher>().player = gameObject;
            
            
            
            
            _animator.Play("brutal kill");
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