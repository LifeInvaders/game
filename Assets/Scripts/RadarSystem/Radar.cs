using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace RadarSystem
{
    public class Radar : MonoBehaviour
    {
        // Start is called before the first frame update
        

        // Update is called once per frame
        [SerializeField] private Transform _target;
        [SerializeField]private RectTransform _radar;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private Transform origin;
        

        public void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetComponentInParent<PhotonView>().IsMine)
                gameObject.SetActive(false);
        }

        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_target == null)
            {
                _spriteRenderer.material.SetFloat("Vector1_A5BC52FF",0);
                text.text = "";
                return;
            }
            float distance = Vector3.Distance(origin.position, _target.position);

            Quaternion quaternion = Quaternion.LookRotation(transform.position - _target.position);
            quaternion.z = -quaternion.y;
            quaternion.y = 0;
            quaternion.x = 0;

            _spriteRenderer.material.SetFloat("Vector1_A5BC52FF",distance);

            if (distance < 20)
            {
                float ydist = origin.position.y - _target.position.y;
                if (ydist <= -3)
                    text.text = "UP";
                else if (ydist >= 3)
                    text.text = "DOWN";
                else
                    text.text = "";
            }
            else
                text.text = "";


            _radar.localRotation = quaternion * Quaternion.Euler(0,0,transform.eulerAngles.y);

        }
    }
}
