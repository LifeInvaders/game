using UnityEngine;
using UnityEngine.AI;

namespace People.NPC.Hiding
{
    public class HidingPray : HidingZone
    {
        protected override void SetTransform()
        {
            transform.parent.eulerAngles = Vector3.up * Random.Range(0, 365f);
        }

        protected override void WherePlaceNpc(WalkingNPC walkingNpc)
        {
            
            float angle = (2 * Mathf.PI) * nextPlace / (NumberOfNpc + 1);
                
                
            walkingNpc.GetComponent<NavMeshAgent>().destination =
                transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        }
    }
}