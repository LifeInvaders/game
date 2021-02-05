using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Objects
{
    public class Doors : MonoBehaviour
    {
        private Transform _leftDoor;

        private Transform _rightDoor;

        private NavMeshObstacle _obstacle;
        private Animator _anim;
        private bool _opened = true;

        private NavMeshObstacle _navMeshObstacle;
        // Start is called before the first frame update
        private void Start()
        {
            _leftDoor = transform.Find("left door");
            _rightDoor = transform.Find("right door");
            _anim = GetComponent<Animator>();
            _navMeshObstacle = GetComponent<NavMeshObstacle>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_opened && other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerControler>().Running())
            {
                _opened = false;
                _anim.Play("doors_animation");
                _navMeshObstacle.enabled = true;
                StartCoroutine(WaitCoroutine());
            }
        }
        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(5);
            _opened = true;
            _navMeshObstacle.enabled = false;
        }
    }
}
