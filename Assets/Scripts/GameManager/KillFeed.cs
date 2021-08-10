
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class KillFeed : MonoBehaviour
    {

        public Text killer;
        public Text killed;

        public void CreateKillFeedEntry(string killer, string killed = "NPC")
        {
            this.killer.text = killer;
            this.killed.text = killed;
            StartCoroutine(FadeAndDestroy());
        }

        IEnumerator FadeAndDestroy()
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}