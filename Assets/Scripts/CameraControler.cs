using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook, maxXLook;
    public Transform camAnchor;
    public bool invertXRotation;
    public Transform Target;
    private float curXRot;
    private Transform Obstruction;
    private float zoomSpeed = 2f;
    private void Start()
    {
        Obstruction = Target; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        // ViewObstructed();
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += Vector3.up * (x * lookSensitivity);
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

                if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * (zoomSpeed * Time.deltaTime));
                }
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, Target.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * (zoomSpeed * Time.deltaTime));
                }
            }
        }
        
    }
}
