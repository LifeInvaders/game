using System;
using People;
using TargetSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Powers
{
    public class Poison : PowerTools
    {
        private SelectedTarget _selectedTarget;
        protected override void SetValues()
        {
            TimeBeforeUse = 15;
            _time = 120;
            _selectedTarget = GetComponent<SelectedTarget>();
            IsShortAction = false;
            TimeToStayOnTheButton = 3;
        }

        protected override bool IsValid()
        {
            return _selectedTarget.IsTarget() &&
                Vector3.Distance(transform.position, _selectedTarget.GetTarget().transform.position) < 2;
        }

        protected override void Action()
        {
            _selectedTarget.GetTarget().GetComponent<HumanEvent>().KilledByPoison();
            _selectedTarget.UpdateSelectedTarget(null,null);
        }
    }
}