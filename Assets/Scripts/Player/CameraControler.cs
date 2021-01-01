using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private bool thirdPerson = true;
    public float lookSensitivity;
    public float minXLook, maxXLook;
    public Transform camAnchor;
    public bool invertXRotation;
    public PlayerControler Player;
    private float curXRot;
    public Transform camera;

    private PlayerControler _playerControler;

    
    // private float zoomSpeed = 2f;

    
    private void Start()
    {
        
        _playerControler = Player.GetComponent<PlayerControler>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ChangePOV()
    {
        if (thirdPerson)
        {
            thirdPerson = false;
            Debug.Log($"{camAnchor.position}{camAnchor.localPosition}");
            Debug.Log($"{camera.position}{camera.localPosition}");
            camera.position = camAnchor.position + Vector3.forward * 0.1f;
        }
        else
        {
            thirdPerson = true;
            camera.position = camAnchor.position + new Vector3(0.4f, 0.02f, -1.64f);
        }
    }

    private void LateUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
            ChangePOV();*/
        if (_playerControler.CanRotate())
        {
            transform.eulerAngles += Vector3.up * (Input.GetAxis("Mouse X") * lookSensitivity);
            curXRot += Input.GetAxis("Mouse Y") * lookSensitivity * (invertXRotation ? 1 : (-1));
        
            Vector3 clampedAngle = camAnchor.eulerAngles;
            clampedAngle.x = Mathf.Clamp(curXRot, minXLook, maxXLook);;
            camAnchor.eulerAngles = clampedAngle;
        }
        
    } 
}
