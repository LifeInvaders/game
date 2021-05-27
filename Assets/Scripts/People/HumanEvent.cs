using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace People
{
    public abstract class HumanEvent : MonoBehaviour
    {
        [SerializeField] protected Material dissolveShader;
        
        [Header("Special finishers")]
        [SerializeField] protected GameObject poisonFinisher;
        protected HumanTasks HumanTask = HumanTasks.Talking;
        public abstract void Death();

        public abstract void TriggeredBySmokeBomb(float endtime);

        public abstract void KilledByPoison();
    }
}