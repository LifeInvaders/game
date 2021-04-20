using UnityEngine;

namespace People
{
    public abstract class Human : MonoBehaviour
    {
        [SerializeField] protected Material dissolveShader;

        public abstract void Death();
    }
}