using System.Collections;
using System.Collections.Generic;
using People;
using UnityEngine;
using UnityEngine.Serialization;

namespace People
{
    public abstract class HumanEvent : MonoBehaviour
    {
        public HumanTasks humanTask = HumanTasks.Nothing;
        
        [SerializeField] protected Material dissolveShader;
        
        [Header("Special finishers")]
        [SerializeField] protected GameObject poisonFinisher;
        
        public abstract void Death();

        public abstract void TriggeredBySmokeBomb(float endtime);

        public abstract void KilledByPoison();
        public abstract void HarmedByKnife();

        public abstract IEnumerator DeathByGun();
    }
}