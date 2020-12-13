using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Vector3;

public class CameraOrbit: MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook, maxXLook;
    public Transform camAnchor;
    public bool invertXRotation;
    public Transform Target, Player;
    private float curXRot;
    public Transform Obstruction;
    private float zoomSpeed = 2f;
    private void Start()
    {
        Obstruction = Target;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        ViewObstructed();
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += up * (x * lookSensitivity);
        if (invertXRotation)
            curXRot += y * lookSensitivity;
        else
            curXRot -= y * lookSensitivity;

        curXRot = Mathf.Clamp(curXRot, minXLook, maxXLook);

        Vector3 clampedAngle = camAnchor.eulerAngles;
        clampedAngle.x = curXRot;
        camAnchor.eulerAngles = clampedAngle;

    }

    private void ViewObstructed()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if (Distance(Obstruction.position, transform.position) >= 3f && Distance(transform.position, Target.position) >= 1.5f)
                {
                    transform.Translate(forward * (zoomSpeed * Time.deltaTime));
                }
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Distance(transform.position, Target.position) < 4.5f)
                {
                    transform.Translate(back * (zoomSpeed * Time.deltaTime));
                }
            }
        }
        
    }
}