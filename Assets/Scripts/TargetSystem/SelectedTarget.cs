using System;
using UnityEngine;

namespace TargetSystem
{
    public class SelectedTarget : MonoBehaviour
    {
        private GameObject _selectedTarget;
        private Outline _outlineTarget;
        private bool _isselectedtarget = false;
        private RaycastHit _raycastHit;

        [SerializeField] private Camera camera;    
        // Start is called before the first frame update


        // Update is called once per frame
        private void ResetOutline()
        {

            if (_selectedTarget != null)
            {
                _outlineTarget.OutlineColor = Color.white;
                _outlineTarget.enabled = false;
                _outlineTarget = null;
            }
        }

        public bool IsTarget()
        {
            return _isselectedtarget;
        }

        public GameObject GetTarget()
        {
            return _selectedTarget;
        }

        public bool IsSelectedTarget(GameObject target)
        {
            
            return _selectedTarget != null && target.name == _selectedTarget.name;
        }

        public void UpdateSelectedTarget(GameObject target,Outline outline)
        {
            ResetOutline();
            if (IsSelectedTarget(target))
            {
                _selectedTarget = null;
                _isselectedtarget = false;
            }
            else
            {
                _selectedTarget = target;
                _outlineTarget = outline;
                _outlineTarget.OutlineColor = Color.yellow;
                _isselectedtarget = true;
            }
        }

        private void Update()
        {
            if (IsTarget() && Vector3.Distance(_selectedTarget.transform.position,transform.position) > 30)
            {
                UpdateSelectedTarget(_selectedTarget, _outlineTarget);
            }
        }
    }
}
