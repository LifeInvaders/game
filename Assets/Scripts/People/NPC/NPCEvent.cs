using System;
using System.Collections;
using System.Collections.Generic;
using People.NPC.Hiding;
using UnityEngine.AI;
using UnityEngine;
namespace People.NPC
{
    public class NPCEvent : HumanEvent
    {
        private NavMeshAgent _navMeshAgent;
        private WalkingNPC _walkingNpc;
        private Animator _animator;
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _walkingNpc = GetComponent<WalkingNPC>();
        }

        public override void Death()
        {
        
            if (_walkingNpc.EventZone != null) 
                _walkingNpc.EventZone.GetComponent<HidingZone>().RemoveDeadNpc(gameObject);
            GetComponentInParent<NpcZone>().GenerateNewNpc();
            Destroy(gameObject);
        }

        public override void TriggeredBySmokeBomb(float endtime)
        {
            Debug.Log($"{endtime}: {gameObject.name}");
            _navMeshAgent.isStopped = true;
            _animator.SetTrigger("smoked");
            StartCoroutine(WaitSmokeBomb(endtime));
        }

        public override void KilledByPoison()
        {
            Debug.Log($"{gameObject.name}");
            StartCoroutine(SpawnPoison());
        }

        private IEnumerator SpawnPoison()
        {
            float speed = _navMeshAgent.speed;
            float time = 0;
            while (time <= 3)
            {
                _navMeshAgent.speed = speed * (1 - time / 3);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            
            var g = Instantiate(poisonFinisher, transform.position, transform.rotation);

            var finisher = g.GetComponent<Finisher>();
            finisher.SetDead(GetComponentInChildren<SkinnedMeshRenderer>());

            Death();
        }
        IEnumerator WaitSmokeBomb(float endtime)
        {
            
            yield return new WaitForSeconds(endtime);
            _navMeshAgent.isStopped = false;
            _animator.SetTrigger("Endsmoked");
        }
    }
}