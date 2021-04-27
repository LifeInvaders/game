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
    public int NumberOfNpc = 0;
    private List<GameObject> NPCs;
    private bool _searching = false;

    private GameObject[] NpcInTheZone;

    private int nextPlace;

    void Start()
    {
        if (NumberOfNpc == 0) NumberOfNpc = Random.Range(2, 4);
        transform.parent.eulerAngles = Vector3.up * Random.Range(0,365f); 
        NPCs = new List<GameObject>();
        NpcInTheZone = new GameObject[NumberOfNpc];
        nextPlace = 0;
        if (NumberOfNpcInTheZone < NumberOfNpc && !_searching)
        {
            StartCoroutine(FindNPC(1));
        }
    }

    public IEnumerator FindNPC(int waiting = 5)
    {
        _searching = true;
        yield return new WaitForSeconds(waiting);
        var nearEvents = Physics.OverlapSphere(transform.position, 15, 512);

        WalkingNPC walkingNpc;
        int i = 0;
        for (; i < nearEvents.Length; i++)
        {
            walkingNpc = nearEvents[i].GetComponent<WalkingNPC>();
            if (walkingNpc.GetStatus() == NpcStatus.Walking)
            {
                NumberOfNpcInTheZone++;
                NpcInTheZone[nextPlace] = nearEvents[i].gameObject;

                walkingNpc.SetStatus(NpcStatus.GoingToEvent);
                walkingNpc.SetEventZone(gameObject);
                float angle = (2 * Mathf.PI) * nextPlace / (NumberOfNpc + 1);
                nextPlace++;
                // Debug.Log(new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
                // float angle = Random.Range(0, Mathf.PI);
                walkingNpc.GetComponent<NavMeshAgent>().destination =
                    transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                break;
            }
        }

        _searching = false;


        if (NumberOfNpcInTheZone < NumberOfNpc)
        {
            StartCoroutine(FindNPC());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var npcData = other.GetComponent<NPCdata>();
            var walkingNpc = other.GetComponent<WalkingNPC>();
            if (npcData.GetStatus() == NpcStatus.GoingToEvent && walkingNpc.EventZone == gameObject)
            {
                // NumberOfNpcInTheZone++;
                NPCs.Add(other.gameObject);

                StartCoroutine(RotateNPC(other.GetComponent<NavMeshAgent>(), other.transform));
                npcData.SetStatus(NpcStatus.Talking);
                // other.transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                other.transform.LookAt(transform, Vector3.up);

                if (NumberOfNpcInTheZone == NumberOfNpc)
                {
                    StartCoroutine(RemoveNPC());
                }
            }
        }
    }

    private IEnumerator RotateNPC(NavMeshAgent agent, Transform Npc)
    {
        float remainingTime = 0f;
        while (remainingTime < 3f || agent.remainingDistance < 2)
        {
            Npc.LookAt(transform);
            remainingTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Npc.LookAt(transform);
    }

    private IEnumerator RemoveNPC()
    {
        yield return new WaitForSeconds(Random.Range(6,15));
        int i = 0;
        for (; i < NumberOfNpc; i++)
            if (NpcInTheZone[i] == NPCs[0])
            {
                nextPlace = i;
                NpcInTheZone[i] = null;
                var walkingNpc = NPCs[0].GetComponent<WalkingNPC>();
                walkingNpc.FindRandomDestination();
                walkingNpc.SetEventZone(null);
                NPCs[0].GetComponent<NPCdata>().SetStatus(NpcStatus.Walking);
                NPCs.RemoveAt(0);
                NumberOfNpcInTheZone--;
                if (!_searching)
                {
                    StartCoroutine(FindNPC());
                }

                break;
            }
    }
}