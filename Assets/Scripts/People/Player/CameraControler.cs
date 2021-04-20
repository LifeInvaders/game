using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Photon.Pun;

namespace People.Player
{
    public class CameraControler : MonoBehaviour
    {
        private bool _thirdPerson = true;
        public float lookSensitivity;
        public float minXLook, maxXLook;
        public Transform camAnchor;
        public bool invertXRotation;

        private float _curXRot;
        private float _xAxis;
        private float _zAxis;

        public CinemachineVirtualCamera camera;

        private PlayerControler _playerControler;


        public void ResetCamera()
        {
            camera.transform.localPosition = new Vector3(0.4f, 0, -1.64f);
        }

        private void Awake()
        {
            camera = GetComponentInChildren<CinemachineVirtualCamera>();
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
            {
                camera.gameObject.SetActive(false);
                enabled = false;
            }
        }

        private void Start()
        {
            _playerControler = GetComponent<PlayerControler>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ChangePOV()
        {
            if (_thirdPerson)
            {
                _thirdPerson = false;
                Debug.Log($"{camAnchor.position}{camAnchor.localPosition}");
                Debug.Log($"{camera.transform.position}{camera.transform.localPosition}");
                camera.transform.position = camAnchor.position + Vector3.forward * 0.1f;
            }
            else
            {
                _thirdPerson = true;
                camera.transform.position = camAnchor.position + new Vector3(0.4f, 0.02f, -1.64f);
            }
        }

        private void LateUpdate()
        {
            if (_playerControler.CanRotate())
            {
                transform.eulerAngles += Vector3.up * (_xAxis * lookSensitivity * .1f);
                _curXRot += _zAxis * lookSensitivity * .1f * (invertXRotation ? 1 : (-1));

                Vector3 clampedAngle = camAnchor.eulerAngles;
                clampedAngle.x = Mathf.Clamp(_curXRot, minXLook, maxXLook);
                ;
                camAnchor.eulerAngles = clampedAngle;
            }
        }

        public void OnCamera(InputValue value)
        {
            Vector2 vector2 = value.Get<Vector2>();
            _xAxis = vector2.x;
            _zAxis = vector2.y;
        }

        public void OnCamAnchor(InputValue value)
        {
            camera.transform.localPosition = new Vector3(-camera.transform.localPosition.x, camera.transform.localPosition.y,
                camera.transform.localPosition.z);
        }
    }
}