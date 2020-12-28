using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook, maxXLook;
    public Transform camAnchor;
    public bool invertXRotation;
    public PlayerControler Player;
    private float curXRot;

    private PlayerControler _playerControler;

    
    // private float zoomSpeed = 2f;

    
    private void Start()
    {
        
        _playerControler = Player.GetComponent<PlayerControler>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
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
