using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed;
    public float walkSpeed = 6;
    public float runSpeed = 3;
    
    private Rigidbody rig;
    private Animator anim;
    private CapsuleCollider capsule;
    public float jumpspeed = 5;
    private bool canRotate = true;
    private bool canMove = true;
    // public float speedClimbing = 1;

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
        rig = GetComponent<Rigidbody>();
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
        if (canMove)
        {
            var right = transform.right;
            var forward = transform.forward;
            SetAnim(x, z);

            Vector3 dir = (right * x + forward * z) * moveSpeed;
            dir.y = rig.velocity.y;
            rig.velocity = dir;

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                anim.SetBool("jump", true);
                rig.AddForce(new Vector3(0, jumpspeed, 0), ForceMode.Impulse);
                anim.SetBool("jump", false);
            }
        }
    }
    void Update()
    {
        if (Input.GetButton("Running"))
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;
        
        Move();
    }
}
