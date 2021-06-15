using UnityEngine;

namespace Tutorial
{
    public class MacroCanvasTutorial : MonoBehaviour
    {
        // Start is called before the first frame update
        private Animator _animator;
        private void Start() => _animator = GetComponent<Animator>();

        public void FadeIn() => gameObject.SetActive(true); 
        public void FadeOut() => _animator.SetTrigger("exit");
    }
}
