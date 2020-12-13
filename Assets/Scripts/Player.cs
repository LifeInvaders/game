using System;
using System.Collections;
using System.Collections.Generic;
// using Packages.Rider.Editor.Util;
using UnityEngine;
// using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed;
    public Rigidbody rig;
    private Animator anim;
    private bool climbing = false;
    private float speedClimbing = 1;
    void Start()
    {
        anim = GetComponent<Animator>();
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
            anim.SetFloat("Speed Front", Mathf.Abs(z)*moveSpeed);
                  
            // Je set les animations de marche latérale
            if (x>=0)
            {
                anim.SetFloat("Speed Side", x*moveSpeed);
                anim.SetBool("Mirror",false);
            }
            else
            {
                anim.SetFloat("Speed Side", -x*moveSpeed);
                anim.SetBool("Mirror",true);
            }
            // CharacterController chr =
            Vector3 dir = (right * x + forward * z) * moveSpeed;
            dir.y = rig.velocity.y;
            rig.velocity = dir;
            // rig.AddForce(dir);
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     rig.AddForce(Vector3.up * 10 * moveSpeed, ForceMode.Impulse);
            // }  
        }
        else
        {
            transform.Translate( Vector3.up * speedClimbing * Time.deltaTime);
        }
    }
    // Update is called once per frame
    private void Jump()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Echelle")
        {
            GetComponent<Rigidbody>().useGravity = false;
            climbing = true;
            
            anim.SetFloat("Speed Front", 0);
            anim.SetFloat("Speed Side", 0);
            
            anim.SetBool("Echelle",true);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Echelle")
        {
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
