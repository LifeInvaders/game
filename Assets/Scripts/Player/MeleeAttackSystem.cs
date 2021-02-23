using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSystem : MonoBehaviour
{
    public Transform attackSphere;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public LayerMask npcLayer;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MeleeAttack();
        }
    }

    
    void MeleeAttack()
    {
        var position = attackSphere.position;
        Collider[] hitnpcs = Physics.OverlapSphere(position,attackRange,npcLayer);
        Collider[] hitplayers = Physics.OverlapSphere(position,attackRange,playerLayer);
        foreach (Collider npc in hitnpcs)
        {
            Debug.Log("You hit " + npc);
            GameObject npcobj = npc.gameObject;
            Destroy(npcobj);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackSphere.position,attackRange);
    }
}
