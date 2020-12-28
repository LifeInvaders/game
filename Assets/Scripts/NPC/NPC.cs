using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Rigidbody rig;
    private Animator anim;
    private NavMeshAgent agent;
    public Transform goal;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        anim.SetBool("walk",true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.destination = goal.position;

        // agent.destination = new Vector3(transform.position.x + Random.Range(-5, 5), 0,
        //     transform.position.x + Random.Range(-20, 20));
    }
    

}
