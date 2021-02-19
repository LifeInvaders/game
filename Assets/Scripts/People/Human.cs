using UnityEngine;

namespace Human
{
    public abstract class Human : MonoBehaviour
    {
        protected Mesh mesh;
        protected Material material;
        
        public abstract void Kill();
        public abstract void Death();
        
    }
}