using System;
using System.Collections;
using People.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tutorial
{
    public class Discussion : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textZone;
        [SerializeField] private GameObject continueZone;
        [SerializeField] private Animator PirateAnimator;
        private bool _hasFinishedToShowText;
        public bool HasExited;
        public PlayerControler playerController;


        public void SetText(string s)
        {
            // PirateAnimator.transform.rotation = new Quaternion(0, 0.9187912f, 0, 0.394744f);
            HasExited = false;
            _hasFinishedToShowText = false;
            continueZone.SetActive(false);
            StartCoroutine(SetTextAnim(s));
        }

        public void SetAnim(string anim)
        {
            PirateAnimator.SetTrigger(anim);
        }

        private IEnumerator SetTextAnim(string text)
        {
            
            textZone.enableAutoSizing = true;
            textZone.text = text;
            yield return new WaitForEndOfFrame();
            // var textZoneFontSize = textZone.fontSize;
            textZone.enableAutoSizing = false;
            
            // textZone.fontSize = textZoneFontSize;
            textZone.text = "";
            foreach (var t in text)
            {
                textZone.text += t;
                yield return new WaitForSeconds(0.05f);
            }

            _hasFinishedToShowText = true;
            continueZone.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && _hasFinishedToShowText)
                Exit();
            
        }

        private void Exit()
        {
            HasExited = true;
            gameObject.SetActive(false);
            playerController.SetMoveBool(true);
            playerController.SetRotateBool(true);
            
            PirateAnimator.SetTrigger("Default");
            Destroy(gameObject);
        }
        
    }
}