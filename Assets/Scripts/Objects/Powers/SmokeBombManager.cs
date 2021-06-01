using System;
using System.Collections;
using People;
using UnityEngine;
namespace Objects.Powers
{
    public class SmokeBombManager : MonoBehaviour
    {
        private GameObject _player;
        private void Start()
        {
            StartCoroutine(EndTimer());
        }

        public void SetPlayer(GameObject player) => _player = player;

        private float _endtime = 11;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") || other.CompareTag("Player") && other.GetInstanceID() != _player.GetInstanceID())
                other.GetComponentInChildren<HumanEvent>().TriggeredBySmokeBomb(_endtime);
        }
        
        IEnumerator EndTimer()
        {
            while (_endtime >0)
            {
                _endtime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
            
        }
    }
}