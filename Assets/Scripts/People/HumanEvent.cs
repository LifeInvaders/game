using UnityEngine;

namespace People
{
    public abstract class HumanEvent : MonoBehaviour
    {
        [SerializeField] protected Material dissolveShader;

        public abstract void Death();
    }
}