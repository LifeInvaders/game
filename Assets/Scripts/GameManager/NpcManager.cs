using Photon.Pun;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

namespace People.NPC
{
    public class NpcManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private int numberOfNpc;
        private Transform[] _points;
        private System.Random _random;

        void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _random = new System.Random();
            var points = transform.Find("POINTS");
            _points = points.GetComponentsInChildren<Transform>();
            for (int i = 1; i <= numberOfNpc; i++)
            {
                var npc =PhotonNetwork.Instantiate("PhotonNPC",
                    _points[i % _points.Length].position /*+ new Vector3(Random.Range(-4,4),0,Random.Range(-4,4))*/,
                    Quaternion.identity);
            }
        }
    }
}