using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace NPC
{
    public class GroupNPC : MonoBehaviour
    {
        public List<GameObject> GetNPC()
        {
            List<GameObject> NPCs = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject npc;
                if ((npc = transform.GetChild(i).gameObject).CompareTag("NPC"))
                    NPCs.Add(npc);
            }

            return NPCs;
        }
    }
}