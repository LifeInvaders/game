using UnityEngine;
using UnityEngine.InputSystem;

namespace TargetSystem
{
    public class KillTarget : MonoBehaviour
    {
        // Start is called before the first frame update
        private SelectedTarget _selectedTarget;
        private Animator _animator;

        private bool _killClick;
        private CastTarget _casttarget;

        void Start()
        {
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
            Debug.Log($"killed {target.name}");
            target.GetComponent<Animator>().Play("brutal death");
            _animator.Play("sword kill");
            _selectedTarget.UpdateSelectedTarget(target,target.GetComponentInChildren<Outline>());
        }
        void Update()
        {
            if (_killClick) // appuye sur Clic Gauche
            {
                _killClick = false;
                GameObject target = _selectedTarget.GetTarget();
                // Si le joueur est à moins d'un mètre et demi.
                if (target != null && Vector3.Distance(target.transform.position,transform.position) < 1.5f)
                {
                    Kill(target);
                    _casttarget.SetAiming(false);

                }
            } 
        }
        
        public void OnAttack(InputValue value)
        {
            _killClick = value.isPressed;
        }
    }
}
