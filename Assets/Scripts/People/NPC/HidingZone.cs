using System;
using System.Collections;
using System.Collections.Generic;
using People.NPC;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class HidingZone : MonoBehaviour
{
    // Start is called before the first frame update
    public int NumberOfNpcInTheZone = 0;
    public int MinNPC = 2;
    public List<GameObject> NPCs;

    void Start()
    {
        NPCs = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var npcData = other.GetComponent<NPCdata>();
            if (npcData.GetStatus() == NpcStatus.GoingToEvent)
            {
                NumberOfNpcInTheZone++;
                NPCs.Add(other.gameObject);

                var agent = other.GetComponent<NavMeshAgent>();
                float remainingTime = 2f;
                while (remainingTime > 0 && !agent.isStopped)
                    remainingTime -= Time.deltaTime;
                npcData.SetStatus(NpcStatus.Talking);
                // other.transform.eulerAngles = transform.eulerAngles;
                other.transform.LookAt(transform);
            
                if (NumberOfNpcInTheZone > MinNPC)
                {
                    NPCs[0].GetComponent<WalkingNPC>().FindRandomDestination();
                    NPCs[0].GetComponent<NPCdata>().SetStatus(NpcStatus.Walking);
                    NPCs.RemoveAt(0);
                    NumberOfNpcInTheZone--;
                }
            }
        }
    }
    
}