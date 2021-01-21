using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public GameObject iaPoints;
    private Transform[] positions;
    void Start()
    {
        positions = iaPoints.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        // agent.autoBraking = false;
        anim = GetComponent<Animator>();
        
        anim.SetBool("walk",true);
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"));
        {
            Debug.Log("coucou");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
        
    }

    private void GotoNextPoint()
    {
        agent.destination = positions[Random.Range(0, positions.Length)].position;
    }
}
