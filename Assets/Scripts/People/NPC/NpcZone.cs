using UnityEngine;
using UnityEditor;

namespace People.NPC
{
    public class NpcZone : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private int numberOfNpc;

        private Transform[] _points;

        [SerializeField] private GameObject prefab;

        private Transform parent;
        private System.Random _random;

        [SerializeField] private float searchRadius = 15;

        void Start()
        {
            _random = new System.Random();
            var points = transform.Find("POINTS");
            _points = points.GetComponentsInChildren<Transform>();

            var walkingNpc = prefab.GetComponent<WalkingNPC>();
            walkingNpc.SetIAPoints(_points);
            walkingNpc.RandomPosition = true;
            walkingNpc.ParentPosition = transform.position;
            walkingNpc.SearchRadius = searchRadius;
        
            parent = transform.Find("NPC");
            for (int i = 1; i <= numberOfNpc; i++)
                Instantiate(prefab, _points[i % _points.Length].position + new Vector3(Random.Range(-2,2),0,Random.Range(-2,2)), Quaternion.identity, parent);
        }

        public void GenerateNewNpc()
        {
            Instantiate(prefab, _points[_random.Next(1,_points.Length)].position, Quaternion.identity, parent);
        }
    }
}