using System;
using System.Collections;
using People;
using TargetSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Objects.Powers
{
    public class PhotonPoison : PowerTools
    {
        private SelectedTarget _selectedTarget;
        private KillTargetPhoton _killTargetPhoton;
        [SerializeField] private int killTime = 5;

        
        protected override void SetValues()
        {
            TimeBeforeUse = 15;
            _time = 120;
            _selectedTarget = GetComponent<SelectedTarget>();
            _killTargetPhoton = GetComponent<KillTargetPhoton>();
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
            StartCoroutine(WaitForKill());
        }

        IEnumerator WaitForKill()
        {
            GameObject target = _selectedTarget.GetTarget();
            yield return new WaitForSeconds(killTime);
            _killTargetPhoton.Kill(target,true);
        }
    }
}