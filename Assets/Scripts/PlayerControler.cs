using System;
using System.Collections;
using System.Collections.Generic;
// using Packages.Rider.Editor.Util;
using UnityEngine;
// using UnityEngine.EventSystems;

public class PlayerControler : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed;
    public Rigidbody rig;
    private Animator anim;
    private bool climbing = false;
    public float speedClimbing = 1;

    public bool returnClimbing()
    {
        return climbing;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Climbing()
    {
        transform.Translate( Vector3.up * (speedClimbing * Time.deltaTime));
    }

    private void SetAnim(float x, float z)
    {
        anim.SetFloat("Speed Front", Mathf.Abs(z)*moveSpeed);
        // Je set les animations de marche latérale
        anim.SetFloat("Speed Side", Mathf.Abs(x)*moveSpeed);
        anim.SetBool("Mirror",x>=0);
        
    }
    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // float y = Input.GetAxis("Jump");
        if (!climbing)
        {
            var right = transform.right;
            var forward = transform.forward;
            SetAnim(x,z);
            // CharacterController chr =
            Vector3 dir = (right * x + forward * z) * moveSpeed;
            dir.y = rig.velocity.y;
            rig.velocity = dir;
        }
        else
        {
            Climbing();
        }
    }
    // Update is called once per frame
    private void Jump()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Echelle"))
        {
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            GetComponent<Rigidbody>().useGravity = false;
            capsule.center = new Vector3(capsule.center.x,1.63f,capsule.center.z);
                climbing = true;
            // transform.rotation = Quaternion.LookRotation(col.gameObject.transform.rotation)
            // Quaternion.RotateTowards(transform.rotation, , Time.deltaTime);
            anim.SetFloat("Speed Front", 0);
            anim.SetFloat("Speed Side", 0);

            anim.SetBool("Echelle",true);
            
            transform.eulerAngles = col.gameObject.transform.eulerAngles;
            
            
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Echelle"))
        {
            
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
            GetComponent<Rigidbody>().useGravity = true;
            climbing = false;
            anim.SetBool("Echelle",false);
            transform.Translate(Vector3.forward *1.3f);
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = 6;
            anim.SetBool("running", true);
        }
        else
        {
            moveSpeed = 3;
            anim.SetBool("running", false);
        }
        Move();
    }
}
