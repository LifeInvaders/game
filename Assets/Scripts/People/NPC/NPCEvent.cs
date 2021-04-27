using System.Collections;
using UnityEngine.AI;
using UnityEngine;
namespace People.NPC
{
    public class NPCEvent : Human
    {   
        
        public override void Death()
        {
            var walkingNpc = GetComponent<WalkingNPC>();
            if (walkingNpc.EventZone != null) 
                walkingNpc.EventZone.GetComponent<HidingZone>().RemoveDeadNpc(gameObject);
            GetComponentInParent<NpcZone>().GenerateNewNpc();
            Destroy(gameObject);
            return;
            Destroy(GetComponent<WalkingNPC>());
            // target.GetComponent<WalkingNPC>().enabled = false;
            // GetComponent<NavMeshAgent>().isStopped = true;
            
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<Animator>().Play("brutal death");

            StartCoroutine(WaitForDeathAnim());
        }
        
        IEnumerator WaitForDeathAnim()
        {
            yield return new WaitForSeconds(5);
            SkinnedMeshRenderer meshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
            Texture oldTexture = meshRenderer.sharedMaterial.mainTexture;

            var mat = new Material(dissolveShader);
            mat.SetTexture("Texture2D_C902C618", oldTexture);
            meshRenderer.sharedMaterial = mat;
            // meshRenderer.sharedMaterial = dissolveMaterial;
            // meshRenderer.sharedMaterial.SetTexture("Texture2D_C902C618", oldTexture);
            meshRenderer.sharedMaterial.SetFloat("Vector1_203537A2", Time.time);


            float timeElapsed = 0f;
            float phase = 3;
            float targetPhase = 5.5f;
            while (timeElapsed <= 3)
            {
                timeElapsed += Time.deltaTime;
                meshRenderer.sharedMaterial.SetFloat("Vector1_203537A2",
                    Mathf.Lerp(phase, targetPhase, timeElapsed / 3));
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }
    }
}