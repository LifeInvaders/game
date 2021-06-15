using System;
using People.Player;
using UnityEngine;

namespace Player
{
    public class EmoteMenu : MonoBehaviour
    {
        private int emote;
        private Animator _animator;

        public void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            emote = PlayerDatabase.Instance.SelectedEmote;
            PlayAnim();
        }

        public void ChangeEmote(int selected)
        {
            emote = selected;
            ApplyChanges();
            PlayAnim();
        }

        private void PlayAnim()
        {
            _animator.SetTrigger($"{emote}");
        }

        public void ApplyChanges()
        {
            PlayerDatabase.Instance.SelectedEmote = emote;
        }
    }
}