using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using People;
using People.NPC;
using People.Player;
using Photon.Pun;
using TargetSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Objects.Powers
{
    public class PhotonKnifeThrowing : PowerTools
    {
        private SelectedTarget _selectedTarget;
        private float _maxDistance;

        private Transform Right_Hand;

        protected override void SetValues()
        {
            Right_Hand = gameObject.transform
                .Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L").transform;
            _time = 90;
            TimeBeforeUse = 0;
            _selectedTarget = GetComponent<SelectedTarget>();
            _maxDistance = 15;
        }

        protected override bool IsValid()
        {
            if (!_selectedTarget.IsTarget())
                return false;
            var distance = Vector3.Distance(transform.position, _selectedTarget.GetTarget().transform.position);
            
            if (distance >= _maxDistance || Physics.Raycast(transform.position,transform.forward,distance,0))
                return false;
            var humanEvent = _selectedTarget.GetTarget().GetComponent<HumanEvent>();
            return humanEvent.humanTask != HumanTasks.Bleeding && humanEvent.humanTask != HumanTasks.SpeedRunning;
        }

        protected override void Action()
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            GetComponent<Animator>().SetTrigger("throw");
            yield return new WaitForSeconds(0.8f);
            
            var target = _selectedTarget.GetTarget();
            var humanEvent = target.GetComponent<HumanEvent>();
            target.GetPhotonView().RPC(nameof(humanEvent.HarmedByKnife),RpcTarget.All);
            PhotonNetwork.Instantiate("knife fx", Right_Hand.position + 0.2f * Vector3.up, Quaternion.LookRotation(Right_Hand.position - target.transform.position));
        }
    }
}