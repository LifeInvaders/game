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
    private bool canRotate = true, canMove = true;
    public float speedClimbing = 1;

    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public bool CanRotate()
    {
        return canRotate;
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
        if (!climbing && canMove)
        {
            var right = transform.right;
            var forward = transform.forward;
            SetAnim(x,z);
            
            Vector3 dir = (right * x + forward * z) * moveSpeed;
            dir.y = rig.velocity.y;
            rig.velocity = dir;
        }
        else
            Climbing();
    }
    // Update is called once per frame
    private void Jump()
    {
        
    }

    private void SetLadder(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Set Ladder animation*/
        GetComponent<Rigidbody>().useGravity = false;
        // capsule.center = new Vector3(capsule.center.x,1.63f,capsule.center.z);

        climbing = true;
        canRotate = false;
            
        anim.SetBool("Echelle",true);
            
        transform.eulerAngles = col.gameObject.transform.eulerAngles;
    }
    private void UnsetLadder(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Go back to normal animation*/
        // capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
        GetComponent<Rigidbody>().useGravity = true;
        canRotate = true;
        climbing = false;
        anim.SetBool("Echelle",false);
        
        // transform.Translate(Vector3.up*0.9f+Vector3.forward);
        transform.Translate(Vector3.forward *1.3f);
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
        if (col.gameObject.CompareTag("Echelle"))
        {
            SetLadder(ref capsule, ref col);
            return;
        }

        if (col.gameObject.CompareTag("Banc"))
        {
            SetBanc(ref capsule, ref col);
            return;
        }
    }
    
    
    private void OnTriggerExit(Collider col)
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (col.gameObject.CompareTag("Echelle"))
        {
            UnsetLadder(ref capsule, ref col);
            return;
        }

        if (col.gameObject.CompareTag("Banc"))
        {
            UnsetBench(ref capsule, ref col);
            return;
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
