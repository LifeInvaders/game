using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

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
        [SerializeField] private bool sphere = true;

        void Start()
        {
            _random = new System.Random();
            var points = transform.Find("POINTS");
            _points = points.GetComponentsInChildren<Transform>();

            var walkingNpc = prefab.GetComponent<WalkingNPC>();
            walkingNpc.SetIAPoints(_points);
            walkingNpc.RandomPosition = true;
            walkingNpc.parentPosition = transform.position;
            walkingNpc.searchRadius = searchRadius;
            walkingNpc.findInSphere = sphere;

            parent = transform.Find("NPC");
            for (int i = 1; i <= numberOfNpc; i++)
                Instantiate(prefab,
                    _points[i % _points.Length].position /*+ new Vector3(Random.Range(-4,4),0,Random.Range(-4,4))*/,
                    Quaternion.identity, parent);
        }

        public void GenerateNewNpc()
        {
            Instantiate(prefab, _points[_random.Next(1, _points.Length)].position, Quaternion.identity, parent);
        }

        private void OnDrawGizmosSelected()
        {
// #if DEBUG
//             Handles.color = new Color(1, 0, 0, 0.2f);
//             Handles.DrawSolidDisc(transform.position, transform.up, searchRadius);
// #endif
        }
    }
}