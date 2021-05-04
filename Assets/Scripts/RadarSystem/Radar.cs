using System;
using Photon.Pun;
using UnityEngine;

namespace RadarSystem
{
    public class Radar : MonoBehaviour
    {
        // Start is called before the first frame update
        

        // Update is called once per frame
        private Transform _target;
        private RectTransform _radar;
        private SpriteRenderer _spriteRenderer;

        public void Start()
        {
            if (PhotonNetwork.IsConnected && !gameObject.GetComponentInParent<PhotonView>().IsMine)
            {
                gameObject.SetActive(false);
                return;
            }
            _radar = GetComponent<RectTransform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
                return;
            }
            float distance = Vector3.Distance(transform.position, _target.position);

            Quaternion quaternion = Quaternion.LookRotation(transform.position - _target.position);
            quaternion.z = -quaternion.y;
            quaternion.y = 0;
            quaternion.x = 0;

            _spriteRenderer.material.SetFloat("Vector1_A5BC52FF",distance);
            
            
            _radar.localRotation = quaternion * Quaternion.Euler(0,0,transform.eulerAngles.y);

        }
    }
}
