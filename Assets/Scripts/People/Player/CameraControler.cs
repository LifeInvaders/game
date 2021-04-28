using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace People.Player
{
    public enum CamStatus
    {
        FirstCam,
        ThirdCamRightShoulder,
        ThirdCamLeftShoulder
    }

    public class CameraControler : MonoBehaviour
    {
        private CamStatus status = CamStatus.ThirdCamRightShoulder;
        private CamStatus _selectedShoulder = CamStatus.ThirdCamRightShoulder;
        
        [Header("Settings")] public float lookSensitivity;
        public float minXLook, maxXLook;
        public Transform camAnchor;
        public bool invertXRotation;

        private float _curXRot;
        private float _xAxis;
        private float _zAxis;

        [SerializeField]  CinemachineVirtualCamera camera;

        [Header("Cameras")]
        [SerializeField] private GameObject camerasParent;
        [SerializeField] private CinemachineVirtualCamera firstCam;
        [SerializeField] private CinemachineVirtualCamera thirdCamRightShoulder;
        [SerializeField] private CinemachineVirtualCamera thirdCamLeftShoulder;
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private bool isSpectator;
        private PlayerControler _playerControler;

        

        private void Awake()
        {
            if (!isSpectator && PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
            {
                camerasParent.SetActive(false);
                enabled = false;
                return;
            }
            camera = thirdCamRightShoulder;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            _playerControler = GetComponent<PlayerControler>();
        }

        private void OnChangePOV()
        {
            if (status != CamStatus.FirstCam)
            {
                
                // cinemachineBrain.
                firstCam.Priority += 1;
                camera.Priority -= 1;
                camera = firstCam;
                status = CamStatus.FirstCam;
                
            }
            else
            {
                if (_selectedShoulder == CamStatus.ThirdCamRightShoulder )
                {
                    status = CamStatus.ThirdCamRightShoulder;
                    camera = thirdCamRightShoulder;
                }
                else
                {
                    status = CamStatus.ThirdCamLeftShoulder;
                    camera = thirdCamLeftShoulder;
                }
                firstCam.Priority -= 1;
                camera.Priority += 1;

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
            if (status != CamStatus.FirstCam)
            {
                if (_selectedShoulder == CamStatus.ThirdCamRightShoulder )
                {
                    _selectedShoulder = CamStatus.ThirdCamLeftShoulder;
                    thirdCamRightShoulder.Priority -= 1;
                    thirdCamLeftShoulder.Priority += 1;
                }
                else
                {
                    _selectedShoulder = CamStatus.ThirdCamRightShoulder;
                    thirdCamLeftShoulder.Priority -= 1;
                    thirdCamRightShoulder.Priority += 1;
                }
            }
        }
    }
}