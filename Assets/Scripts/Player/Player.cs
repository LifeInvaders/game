using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

	private Animator anim;

	private Camera mainCamera;
	private float velocity;
	private Rigidbody rig;
	private CapsuleCollider capsule;
	public float speedClimbing = 1;
	private float moveSpeed = 3;
	public float jumpspeed = 5;
	// public Transform cam;
	private bool _climb = false;

	private Vector3 targetDirection;
	private Quaternion freeRotation;

	
	// Use this for initialization
	void Start()
	{
		mainCamera = Camera.main;
		// cam = GetComponent<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		anim = GetComponent<Animator>();
		// mainCamera = Camera.main;
		rig = GetComponent<Rigidbody>();
		capsule = GetComponent<CapsuleCollider>();
	}

	private bool IsGrounded()
	{
		var bounds = capsule.bounds;
		return Physics.Raycast(bounds.center, Vector3.down, bounds.extents.y + 0.1f);
	}

	private void Climbing()
	{
		transform.Translate(Vector3.up * (speedClimbing * Time.deltaTime));
	}
	private void SetAnim(float x, float z)
	{
		anim.SetFloat("Speed Front", Mathf.Abs(z)*moveSpeed);
		// Je set les animations de marche latérale
		anim.SetFloat("Speed Side", Mathf.Abs(x)*moveSpeed);
		anim.SetBool("Mirror",x>=0);
		anim.SetBool("running", moveSpeed>3);
        
	}

	void test()
	{
		// Vector3 lookDirection = targetDirection.normalized;
		// freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
		// var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
		// var eulerY = transform.eulerAngles.y;
		//
		// if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
		// var euler = new Vector3(0, eulerY, 0);
		//
		// transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
	}
	
	// Update is called once per frame
	void camera(float x, float z)
	{
		var forward = mainCamera.transform.TransformDirection(Vector3.forward);
		forward.y = 0;

		//get the right-facing direction of the referenceTransform
		var right = mainCamera.transform.TransformDirection(Vector3.right);

		// determine the direction the player will face based on input and the referenceTransform's right and forward directions
		targetDirection = x * right + z * forward;
	}
	void Update()
	{
		if (_climb)
			Climbing();
		else
		{
			moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 6 : 3;
			float x = Input.GetAxis("Horizontal") * moveSpeed;
			float z = Input.GetAxis("Vertical")* moveSpeed;
		
			Vector3 dir = (transform.right * x + transform.forward * z);
			dir.y = rig.velocity.y;
			// rig.velocity = dir;
			SetAnim(x, z);
			// Debug.Log();
			rig.MovePosition(rig.position + dir * Time.deltaTime);
			// Debug.Log($"{Input.GetAxis("Mouse X")}{Input.GetAxis("Mouse Y")}")
			if (x != 0 || z != 0)
			{
				// came.y *= Time.deltaTime;
				// transform.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y * Time.deltaTime, 0);
				
				Vector3 lookDirection = targetDirection.normalized;
				freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
				var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
				var eulerY = transform.eulerAngles.y;

				if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
				var euler = new Vector3(0, eulerY, 0);

				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), Time.deltaTime);
				
			}
			if (Input.GetButtonDown("Jump"))
			{
				if (IsGrounded())
				{
					rig.AddForce(new Vector3(0, jumpspeed, 0), ForceMode.Impulse);
				}
			}
		}
		
	}
	
	 private void SetLadder(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Set Ladder animation*/
        GetComponent<Rigidbody>().useGravity = false;
        // capsule.center = new Vector3(capsule.center.x,1.63f,capsule.center.z);
        _climb = true;
        anim.SetBool("Echelle",true);
            
        transform.eulerAngles = col.gameObject.transform.eulerAngles;
    }
    private void UnsetLadder(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Go back to normal animation*/
        // capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
        GetComponent<Rigidbody>().useGravity = true;
        _climb = false;
        anim.SetBool("Echelle",false);
        
        // transform.Translate(Vector3.up*0.9f+Vector3.forward);
        transform.Translate(Vector3.forward *1.3f);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    /*private void SetBanc(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Set Sitting animation#1#
        anim.SetBool("sitting",true);
        capsule.center = new Vector3(capsule.center.x,0.95f,capsule.center.z);
        capsule.height = 1.1f;
        transform.eulerAngles = col.gameObject.transform.eulerAngles + 180 * Vector3.up;
        transform.position = col.gameObject.transform.position;
        // transform.rotation.x += 180;
        // canMove = false;
    }

    private void UnsetBench(ref CapsuleCollider capsule, ref Collider col)
    {
        /*Go back to normal animation#1#
        // canMove = true;
        anim.SetBool("sitting",false);
        capsule.center = new Vector3(capsule.center.x,0.9f,capsule.center.z);
        capsule.height = 1.97f;
    }*/
    
    
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
            // SetBanc(ref capsule, ref col);
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
            // UnsetBench(ref capsule, ref col);
            return;
        }
        
    }
    
    
}



