using System;
using System.Collections;
using People.Player;
using UnityEngine;

namespace Objects
{
    public class Zipline : MonoBehaviour
    {
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;
        private Vector3 translateVector3;
        private bool isMoving;

        public void Start()
        {
            translateVector3 = endPos.position - startPos.position;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer")) // si collision avec autre joueur
            {
                PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
                other.gameObject.GetComponent<Rigidbody>().useGravity = false; // on désactive la gravité du joueur
                player.SetRotateBool(false);
                player.SetMoveBool(false); // on désactive les mouvements
                player.transform.position = startPos.position;
                player.transform.rotation =
                    Quaternion.Euler(0, startPos.rotation.eulerAngles.y,
                        0); // on tourne le personnage de façon adéquate
                isMoving = true;
                StartCoroutine(MovingOn(other.gameObject));
            }
        }

        private IEnumerator MovingOn(GameObject player)
        {
            PlayerControler playerControler = player.GetComponent<PlayerControler>();
            while(player)
            {
                player.transform.Translate(translateVector3.normalized * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        
    }
}