using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using People;
using People.Player;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Objects
{
    public class Ladder : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cam;

        private List<int> _ejectedPlayers;
        // Start is called before the first frame update
        private void Start()
        {
            _ejectedPlayers = new List<int>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer"))
            {
                var playerEvent = other.GetComponent<HumanEvent>();
                if (IsNotValid(playerEvent)) return;
                cam.Follow = other.transform;
                cam.LookAt = other.transform;
                cam.Priority = 40;
                var transformLocalPosition = other.gameObject.GetComponent<CameraControler>();
                // transformLocalPosition.camera.transform.localPosition = new Vector3(transformLocalPosition.camera.transform.localPosition.x, -0.5f, -2.5f);

                var eulerAngles = transformLocalPosition.camAnchor.transform.eulerAngles;
                eulerAngles = new Vector3(0, eulerAngles.y, eulerAngles.z);
                transformLocalPosition.camAnchor.transform.eulerAngles = eulerAngles;

                playerEvent.humanTask = HumanTasks.Climbing;
                PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                player.SetRotateBool(false);
                player.SetMoveBool(false);
                player.SetCanRun(false);
                other.gameObject.GetComponent<Animator>().SetBool("Echelle", true);
                // other.gameObject.transform.position = new Vector3(transform.position.x,other.gameObject.transform.position.y,transform.position.z);


                // other.gameObject.transform.position = transform.parent.position;
                other.gameObject.transform.eulerAngles = transform.eulerAngles;


                if (gameObject.transform.transform.eulerAngles.x != 0)
                {
                    // other.gameObject.GetComponent<CapsuleCollider>().center += Vector3.forward * 0.1f;
                }
            }
        }

        private static bool IsNotValid(HumanEvent playerEvent) =>
            playerEvent.humanTask == HumanTasks.SpeedRunning || playerEvent.humanTask == HumanTasks.Bleeding;

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer"))
            {
                var playerEvent = other.GetComponent<HumanEvent>();
                switch (playerEvent.humanTask)
                {
                    case HumanTasks.Bleeding:
                    {
                        _ejectedPlayers.Remove(other.GetInstanceID());
                        return;
                    }
                    case HumanTasks.SpeedRunning:
                        return;
                    case HumanTasks.Climbing:
                        other.GetComponent<PlayerControler>().SetCanRun(true);
                        ExitLadder(other.gameObject);
                        playerEvent.humanTask = HumanTasks.Nothing;
                        other.gameObject.transform.Translate(Vector3.forward * 1.3f);
                        
                        cam.Follow = null;
                        cam.LookAt = null;
                        cam.Priority = 0;
                        break;
                }
                
            }
        }

        private bool IsOnTop(GameObject player)
        {
            CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
            return !Physics.Raycast(capsule.bounds.max, Vector3.left, capsule.bounds.extents.y + 0.2f);
        }

        private bool IsGrounded(GameObject player)
        {
            CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
            return Physics.Raycast(capsule.bounds.center, Vector3.down, capsule.bounds.extents.y + 0.2f);
        }

        private void ExitLadder(GameObject playerGameObject)
        {
            PlayerControler player = playerGameObject.GetComponent<PlayerControler>();
            playerGameObject.GetComponent<Rigidbody>().useGravity = true;
            player.SetRotateBool(true);
            player.SetMoveBool(true);

            playerGameObject.GetComponent<Animator>().SetBool("Echelle", false);

            playerGameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        private void JumpFromLadder(GameObject playerGameObject)
        {
            playerGameObject.GetComponent<Animator>().enabled = true;
            PlayerControler player = playerGameObject.GetComponent<PlayerControler>();
            player.SetRotateBool(true);
            player.SetMoveBool(true);

            playerGameObject.GetComponent<Animator>().SetBool("Echelle", false);
            // playerGameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            // playerGameObject.transform.Translate(Vector3.forward * 0.1f);
            playerGameObject.GetComponent<Rigidbody>().AddForce(5 * Vector3.up + Vector3.back * 2, ForceMode.Impulse);
        }

        private IEnumerator Falling(GameObject player)
        {
            var up = transform.up;
            Physics.Raycast(player.transform.position, -up, out var raycastHit, 30, 0);

            var rigidbody = player.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.AddForce(-up * (raycastHit.distance * 0.5f), ForceMode.Impulse);
            rigidbody.AddForce(-transform.forward *100,ForceMode.Force);

            var animator = player.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetTrigger("falling");
            animator.SetFloat("Speed Front",0);
            animator.SetBool("Echelle",false);

            yield return new WaitForSeconds(3);
            var playerControler = player.GetComponent<PlayerControler>();
            
            cam.Follow = null;
            cam.LookAt = null;
            cam.Priority = 0;
            
            playerControler.SetRotateBool(true);
            playerControler.SetMoveBool(true);
            playerControler.CheckIfRunning();
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("MyPlayer"))
            {
                var playerEvent = other.GetComponent<HumanEvent>();
                switch (playerEvent.humanTask)
                {
                    case HumanTasks.SpeedRunning:
                        return;
                    case HumanTasks.Bleeding:
                    {
                        var instanceID = other.GetInstanceID();
                        if (!_ejectedPlayers.Contains(instanceID))
                        {
                            _ejectedPlayers.Add(instanceID);
                            StartCoroutine(Falling(other.gameObject));
                        }
                        return;
                    }
                }

                float input = other.GetComponent<PlayerControler>().GetAxis().y;
                if (input == 0)
                    other.gameObject.GetComponent<Animator>().enabled = false;
                else if (input < 0 && IsGrounded(other.gameObject))
                {
                    ExitLadder(other.gameObject);
                    other.gameObject.transform.Translate(Vector3.back * 1.3f);
                }
                // else if (input < 0 && Input.GetButtonDown("Jump"))
                // {
                //     // JumpFromLadder(other.gameObject);
                // }
                else
                {
                    other.gameObject.GetComponent<Animator>().enabled = true;
                    other.gameObject.transform.Translate(Vector3.up * (Time.deltaTime) * input);
                }
            }
        }
    }
}