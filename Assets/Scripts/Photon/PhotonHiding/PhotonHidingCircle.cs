using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace People.NPC.Hiding
{
    public class PhotonHidingCircle : PhotonHidingZone
    {
        protected override void SetTransform()
        {
            Random.InitState(gameObject.GetPhotonView().ViewID);
            transform.parent.eulerAngles = Vector3.up * Random.Range(0, 365f);
        }

        protected override void WherePlaceNpc(PhotonNPC walkingNpc)
        {
            
            float angle = (2 * Mathf.PI) * nextPlace / (NumberOfNpc + 1);
                
                
            walkingNpc.GetComponent<NavMeshAgent>().destination =
                transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        }
    }
}