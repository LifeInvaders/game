using UnityEngine;

namespace People.Player
{
    public class AnimControler : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnim(string animName, bool isInterracting, float duration = 0.2f)
        {
            _animator.SetBool("isInterracting",isInterracting);
            _animator.CrossFade(animName, duration);
        }
    }
}