using System.Collections;
using System.Collections.Generic;
using People;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class NpcEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int numberOfNpc;

    private Transform[] _points;

    [SerializeField] private GameObject prefab;

    private Transform parent;
    private Random _random;

    void Start()
    {
        _random = new Random();
        var points = transform.Find("POINTS");
        _points = points.GetComponentsInChildren<Transform>();

        var walkingNpc = prefab.GetComponent<WalkingNPC>();
        walkingNpc.SetIAPoints(_points);
        walkingNpc.RandomPosition = true;

        parent = transform.Find("NPC");
        for (int i = 0; i < numberOfNpc; i++)
            Instantiate(prefab, _points[i % _points.Length].position, Quaternion.identity, parent);
    }

    public void GenerateNewNpc()
    {
        Instantiate(prefab, _points[_random.Next(_points.Length)].position, Quaternion.identity, parent);
    }
}