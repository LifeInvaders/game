using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using People.Player;
using UnityEngine;
using UnityEngine.AI;


namespace Objects.Powers
{
    public class SmokeBomb : PowerTools
    {
        [SerializeField] private float _maxDistanceRange;
        [SerializeField] private GameObject smoke;

        public void Start()
        {
            _time = 45;
            TimeBeforeUse = 3;
            _maxDistanceRange = 3;
        }

        protected override bool IsValid() => true;

        protected override void Action()
        {
            var bomb = Instantiate(smoke, transform.position, Quaternion.Euler(-90, 0, 0));

            var hitColliders = Physics.OverlapSphere(transform.position, _maxDistanceRange, 768)
                .Where(p => p.transform != transform); // 1<< 8 | 1<< 9
            hitColliders.Where(p => p.CompareTag("NPC")).ToList()
                .ForEach(e => e.gameObject.GetComponent<NavMeshAgent>().isStopped = true);
            hitColliders.Where(p => p.CompareTag("Player")).ToList()
                .ForEach(e => e.gameObject.GetComponent<PlayerControler>().SetMoveBool(true));
            //TODO : Add Animation here
            StartCoroutine(WaitCoroutine(hitColliders, bomb));
        }

        IEnumerator WaitCoroutine(IEnumerable<Collider> hitColliders, GameObject bomb)
        {
            yield return new WaitForSeconds(7);
            hitColliders.Where(p => p.CompareTag("NPC")).ToList()
                .ForEach(e => e.gameObject.GetComponent<NavMeshAgent>().isStopped = false);
            hitColliders.Where(p => p.CompareTag("Player")).ToList()
                .ForEach(e => e.gameObject.GetComponent<PlayerControler>().SetMoveBool(false));

            Destroy(bomb);
        }
    }
}