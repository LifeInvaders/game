using UnityEngine;

namespace RadarSystem
{
    public class Radar : MonoBehaviour
    {
        public GameObject target;
        // Start is called before the first frame update
        

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 20)
            {
                
            }
        }
    }
}
