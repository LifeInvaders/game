using UnityEngine;

namespace RadarSystem
{
    public class Radar : MonoBehaviour
    {
        // Start is called before the first frame update
        

        // Update is called once per frame
        // TODO : REMOVE SerializeField for target Transform
        [SerializeField] private Transform target;
        [SerializeField] private RectTransform radar;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetTarget(Transform targetTransform)
        {
            target = targetTransform;
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            if (target == null)
            {
                _spriteRenderer.material.SetFloat("Vector1_A5BC52FF",0);
                return;
            }
            float distance = Vector3.Distance(transform.position, target.position);

            Quaternion quaternion = Quaternion.LookRotation(transform.position - target.position);
            quaternion.z = -quaternion.y;
            quaternion.y = 0;
            quaternion.x = 0;

            _spriteRenderer.material.SetFloat("Vector1_A5BC52FF",distance);
            
            
            radar.localRotation = quaternion * Quaternion.Euler(0,0,transform.eulerAngles.y);

        }
    }
}
