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
    public float walkSpeed = 6;
    public float runSpeed = 3;
    
    public Rigidbody rig;
    private Animator anim;
    private CapsuleCollider capsule;
    public float jumpspeed = 5;
    private bool canRotate = true;
    private bool canMove = true;
    public float speedClimbing = 1;

    public void SetMoveBool(bool state)
    {
        canMove = state;
    }
    public void SetRotateBool(bool state)
    {
        canRotate = state;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
    }
    

    public void SetJumpSpeed(float speed)
    {
        jumpspeed = speed;
    }

    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;
    }

    public void SetRunSpeed(float speed)
    {
        runSpeed = speed;
    }
    public bool CanRotate()
    {
        return canRotate;
    }
    public bool Running()
    {
        return moveSpeed>walkSpeed;
    }

    private void SetAnim(float x, float z)
    {
        anim.SetFloat("Speed Front", Mathf.Abs(z)*moveSpeed);
        // Je set les animations de marche latérale
        anim.SetFloat("Speed Side", Mathf.Abs(x)*moveSpeed);
        anim.SetBool("Mirror",x<0);
        anim.SetBool("running", moveSpeed>walkSpeed);
    }
    
    private bool IsGrounded()
    {
        return Physics.Raycast(capsule.bounds.center, Vector3.down,capsule.bounds.extents.y+0.2f);
    }
    
    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // float y = Input.GetAxis("Jump");
        if (canMove)
        {
            var right = transform.right;
            var forward = transform.forward;
            SetAnim(x,z);
            
            Vector3 dir = (right * x + forward * z) * moveSpeed;
            dir.y = rig.velocity.y;
            rig.velocity = dir;
            
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                anim.SetBool("jump", true);
                rig.AddForce(new Vector3(0,jumpspeed,0),ForceMode.Impulse);
                anim.SetBool("jump", false);
            }
        }
    }
    private void SetBanc(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Set Sitting animation*/
        anim.SetBool("sitting",true);
        capsule.center = new Vector3(capsule.center.x,0.95f,capsule.center.z);
        capsule.height = 1.1f;
        canRotate = false;
        transform.eulerAngles = col.gameObject.transform.eulerAngles + 180 * Vector3.up;
        transform.position = col.gameObject.transform.position;
        // transform.rotation.x += 180;
        // canMove = false;
    }

    private void UnsetBench(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Go back to normal animation*/
        canRotate = true;
        // canMove = true;
        anim.SetBool("sitting",false);
        capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
        capsule.height = 1.97f;
    }
    
    
    private void OnTriggerEnter(Collider col)
    {
        /*Regarde avec quel objet il est en collision, et applique les animations et modification adaptées*/
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (col.gameObject.CompareTag("Banc"))
        {
            // SetBanc(ref capsule, ref col);
            return;
        }
        
    }
    
    
    private void OnTriggerExit(Collider col)
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (col.gameObject.CompareTag("Banc"))
        {
            // UnsetBench(ref capsule, ref col);
            return;
        }
    }
    
    
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;
        
        Move();
    }
}
