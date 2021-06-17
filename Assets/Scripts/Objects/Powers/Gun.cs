using People;
using TargetSystem;
using UnityEngine;

namespace Objects.Powers
{
    public class Gun : PowerTools
    {
        private SelectedTarget _selectedTarget;
        private float _maxDistance = 30;

        private void Start()
        {
            _selectedTarget = GetComponent<SelectedTarget>();
        }

        protected override bool IsValid()
        {
            if (!_selectedTarget.IsTarget())
                return false;
            var distance = Vector3.Distance(transform.position, _selectedTarget.GetTarget().transform.position);
            
            if (distance >= _maxDistance || Physics.Raycast(transform.position,transform.forward,distance,0))
                return false;
            var humanEvent = _selectedTarget.GetTarget().GetComponent<HumanEvent>();
            return humanEvent.humanTask != HumanTasks.SpeedRunning;
        }

        protected override void Action()
        {
            StartCoroutine(_selectedTarget.GetTarget().GetComponent<HumanEvent>().DeathByGun());
        }
    }
}