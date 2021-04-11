using System;
using System.Collections;
using System.Collections.Generic;
using People.Player;
using UnityEngine;
using UnityEngine.AI;


namespace Objects.Powers
{
    public class SmokeBomb : PowerTools
    {
        [SerializeField] private float _maxDistanceRange;
        private Transform _transform;

        public void Start()
        {
            _time = 60;
            TimeBeforeUse = 0;
            _maxDistanceRange = 2;
            _transform = GetComponent<Transform>();
        }

        protected override void Action()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxDistanceRange);
            List<GameObject> characters = new List<GameObject>();


            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Player") && hitCollider.gameObject != gameObject)
                {
                    characters.Add(hitCollider.gameObject);
                    hitCollider.gameObject.GetComponent<PlayerControler>().SetMoveBool(false);
                }
                else if (hitCollider.gameObject.CompareTag("NPC"))
                {
                    characters.Add(hitCollider.gameObject);
                    hitCollider.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                }
                
            }

            if (characters.Count > 0)
                StartCoroutine(WaitCoroutine(characters));
        }

        IEnumerator WaitCoroutine(List<GameObject> characters)
        {
            yield return new WaitForSeconds(6);
            
            foreach (var character in characters)
            {
                if (character.gameObject.CompareTag("Player"))
                {
                    characters.Add(character.gameObject);
                    character.gameObject.GetComponent<PlayerControler>().SetMoveBool(true);
                }
                else if (character.gameObject.CompareTag("NPC"))
                {
                    characters.Add(character.gameObject);
                    character.gameObject.GetComponent<NavMeshAgent>().isStopped = true;

                }
                // else if (character.gameObject.CompareTag("GroupNPC"))
                // {
                //     character.gameObject.GetComponent<GroupNPC>().GetNPC().ForEach(p =>p.GetComponent<>);
                // }
            }
        }

        protected override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}