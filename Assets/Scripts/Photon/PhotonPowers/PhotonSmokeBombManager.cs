using System.Collections;
using People;
using Photon.Pun;
using UnityEngine;
namespace Objects.Powers
{
    public class PhotonSmokeBombManager : MonoBehaviour
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
            if (other.CompareTag("MyPlayer") && !gameObject.GetPhotonView().IsMine || other.CompareTag("NPC") && PhotonNetwork.IsMasterClient)
                other.GetComponentInChildren<HumanEvent>().TriggeredBySmokeBomb(_endtime);
        }
        
        IEnumerator EndTimer()
        {
            while (_endtime >0)
            {
                _endtime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (gameObject.GetPhotonView().IsMine) PhotonNetwork.Destroy(gameObject);
            
        }
    }
}