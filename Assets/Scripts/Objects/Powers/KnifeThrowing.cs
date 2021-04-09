using System.Collections;
using People.Player;
using TargetSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Objects.Powers
{
    public class KnifeThrowing : PowerTools
    {
        private SelectedTarget _selectedTarget;
        private float _maxDistance;
        [SerializeField] private GameObject fx;

        private void Start()
        {
            _time = 1;
            TimeBeforeUse = 0;
            _selectedTarget = GetComponent<SelectedTarget>();
            _maxDistance = 15;
        }

        protected override bool IsValid() =>
            _selectedTarget.IsTarget() &&
            Vector3.Distance(transform.position, _selectedTarget.GetTarget().transform.position) < _maxDistance;

        protected override void Action()
        {
            var target = _selectedTarget.GetTarget();
            if (target.CompareTag("NPC"))
            {
                target.GetComponent<NavMeshAgent>().speed = 0.8f;
            }
            else if (target.CompareTag("Player"))
            {
                target.GetComponent<PlayerControler>().SetWalkSpeed(1.2f);
                target.GetComponent<PlayerControler>().SetCanRun(false);
            }
            target.GetComponent<Animator>().SetBool("injured",true);
            var fxInstance = Instantiate(fx, transform.position + 0.2f * Vector3.up, Quaternion.LookRotation(transform.position - target.transform.position));
            StartCoroutine(WaitFor(target,fxInstance));
        }


        private static IEnumerator WaitFor(GameObject target, GameObject fxInstance)
        {
            yield return new WaitForSeconds(5);
            Destroy(fxInstance);
            target.GetComponent<Animator>().SetBool("injured",false);
            if (target.CompareTag("NPC"))
            {
                target.GetComponent<NavMeshAgent>().speed = 2;
            }
            else if (target.CompareTag("Player"))
            {
                target.GetComponent<PlayerControler>().SetWalkSpeed(3);
                target.GetComponent<PlayerControler>().SetCanRun(true);
            }
        }
    }
}