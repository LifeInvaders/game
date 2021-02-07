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

        void Start()
        {
            _animator = GetComponent<Animator>();
            _selectedTarget = GetComponent<SelectedTarget>();
        }

        void Kill(GameObject target)
        {
            Debug.Log($"killed {target.name}");
            target.GetComponent<Animator>().Play("brutal death");
            _animator.Play("sword kill");
            _selectedTarget.UpdateSelectedTarget(target,target.GetComponentInChildren<Outline>());
        }
        void Update()
        {
            if (_killClick)
            {
                GameObject target = _selectedTarget.GetTarget();
                if (target != null && Vector3.Distance(target.transform.position,transform.position) < 1.5f)
                {
                    Kill(target);
                }
            } 
        }
        
        public void OnAttack(InputValue value)
        {
            _killClick = value.isPressed;
        }
    }
}
