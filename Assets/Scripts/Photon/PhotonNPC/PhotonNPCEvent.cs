using System;
using System.Collections;
using People.NPC.Hiding;
using Photon.Pun;
using UnityEngine;

namespace People.NPC
{
    public class PhotonNPCEvent : HumanEvent
    {
        private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private PhotonNPC _npc;
        private void Start()
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _animator = GetComponent<Animator>(); 
            _npc = GetComponent<PhotonNPC>();
        }

        public override void Death()
        {
            if (_npc.GetEventZone() != null)
                _npc.GetEventZone().GetComponent<PhotonHidingZone>().RemoveDeadNpc(gameObject);
            if (PhotonNetwork.IsMasterClient)PhotonNetwork.Destroy(gameObject);
        }

        public override void TriggeredBySmokeBomb(float endtime)
        {
            _animator.SetTrigger("smoked");
            _npc.BootUp(11);
            humanTask = HumanTasks.Poisoned;
        }

        public override void KilledByPoison()
        {
            StartCoroutine(SpawnPoison());
        }

        [PunRPC]
        public override void HarmedByKnife()
        {
            var value = _navMeshAgent.speed;
            _navMeshAgent.speed = 1.2f;
            _animator.SetTrigger("injured");
            humanTask = HumanTasks.Bleeding;
            StartCoroutine(WaitKnife(value));
        }

        public override IEnumerator DeathByGun()
        {
            yield return new WaitForSeconds(2);
            
        }

        private IEnumerator WaitKnife(float value)
        {
            yield return new WaitForSeconds(5);
            _navMeshAgent.speed = value;
            _animator.SetTrigger("Default");
            humanTask = HumanTasks.Nothing;
        }

        private IEnumerator SpawnPoison()
        {
            throw new NotImplementedException();
        }
    }
}