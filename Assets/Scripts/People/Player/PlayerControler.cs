using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace People.Player
{
    public class PlayerControler : MonoBehaviour
    {
        // Start is called before the first frame update
        private float _moveSpeed;
        [SerializeField] private float walkSpeed = 3;
        [SerializeField] private float runSpeed = 6;
        [SerializeField] private float jumpspeed = 5;

        private Vector2 _axis;
        private bool _isRunning = false;
        
        public bool IsJumping { get; private set; }

        private Rigidbody _rig;
        private Animator _anim;
        private CapsuleCollider _capsule;

        private bool _canRotate = true;
        private bool _canMove = true;
        private bool _canRun = true;

        private bool _isInAir;
        private bool _isGrounded;
        private float _timeInAir;
        [SerializeField] private float fallingSpeed = 5;
        public void SetMoveBool(bool state) => _canMove = state;
        public void SetCanRun(bool state) => _canRun = state;

        public bool CanRun() => _canRun;

        public void SetRotateBool(bool state) => _canRotate = state;
        
        [SerializeField] private bool isSpectator;

        void Start()
        {
            _axis = Vector2.zero;
            _rig = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
            _capsule = GetComponent<CapsuleCollider>();
            _moveSpeed = walkSpeed;
            if (!isSpectator && PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
            {
                gameObject.GetComponent<PlayerInput>().enabled = false;
                _rig.isKinematic = true;
                enabled = false;
            }
        }


        public void SetJumpSpeed(float speed) => jumpspeed = speed;

        public void SetWalkSpeed(float speed) => walkSpeed = speed;

        public void SetRunSpeed(float speed) => runSpeed = speed;

        public bool CanRotate() => _canRotate;

        public bool Running() => _moveSpeed > walkSpeed;
        public bool Moving() => _axis != Vector2.zero;

        public Vector2 GetAxis() => _axis;

        private void SetAnim()
        {
            _anim.SetFloat("Speed Front", Mathf.Abs(_axis.y) * _moveSpeed);
            // set les animations de marche latérale
            _anim.SetFloat("Speed Side", Mathf.Abs(_axis.x) * _moveSpeed);
            _anim.SetBool("Mirror", _axis.x < 0);
            _anim.SetBool("running", _moveSpeed > walkSpeed);
        }

        private void IsGrounded()
        {
            _isGrounded = Physics.Raycast(_capsule.bounds.center, Vector3.down, _capsule.bounds.extents.y + 0.2f);
            // Debug.Log("time in air : " + _timeInAir);
            // if (!_isGrounded)
            // {
            //     _timeInAir += Time.deltaTime;
            //     if (_timeInAir >= 0.8f && _timeInAir < 1f)
            //     {
            //         _anim.SetBool("falling", true);
            //         _anim.CrossFade("falling", 0.5f);
            //     }
            //
            //     if (_timeInAir >= 1)
            //     {
            //         _canMove = false;
            //         // _rig.AddForce(Vector3.down * fallingSpeed * _timeInAir);
            //         // _rig.AddForce(transform.forward * 1.1f);
            //         Debug.DrawRay(transform.position, -transform.up);
            //         if (Physics.Raycast(transform.position, -transform.up, 0.4f))
            //         {
            //             _anim.SetBool("falling", false);
            //             _canMove = true;
            //         }
            //     }
            // }
            // else
            // {
            //     _timeInAir = 0;
            //     _anim.SetBool("falling", false);
            // }
        }

        private void Move()
        {
            SetAnim();
            Vector3 dir = (transform.right *_axis.x  + transform.forward * _axis.y) * _moveSpeed;
            dir.y = _rig.velocity.y;
            _rig.velocity = dir;
        }

        private void Jump()
        {
            _rig.AddForce(new Vector3(0, jumpspeed, 0), ForceMode.Impulse);
            _anim.SetBool("jump", true);
            StartCoroutine(JumpAnim());
        }

        private IEnumerator JumpAnim()
        {
            
            yield return new WaitForSeconds(1);
            _anim.SetBool("jump", false);
            // float time = 0;
            // while (!_isGrounded)
            // {
            //     _canMove = false;
            //     time += Time.deltaTime;
            //     yield return new WaitForEndOfFrame();
            //     if (time > 2.5f)
            //         break;
            // }
            //
            // _canMove = true;
        }

        void FixedUpdate()
        {
            IsGrounded();
            if (_canMove)
            {
                Move();
            }
        }

        public void OnMovement(InputValue value) => _axis = value.Get<Vector2>();

        public void OnJump()
        {
            if (_isGrounded)
                Jump();
            StartCoroutine(PressingJumpMacro());
        }

        private IEnumerator PressingJumpMacro()
        {
            IsJumping = true;
            yield return new WaitForEndOfFrame();
            IsJumping = false;
        }

        public void OnRun(InputValue value)
        {
            _isRunning = value.Get<float>().Equals(1);
            _moveSpeed = _isRunning && _canRun ? runSpeed : walkSpeed;
        }

        public void CheckIfRunning() =>  _moveSpeed = _canRun && _isRunning ? runSpeed : walkSpeed;

        public float GetWalkSpeed() => walkSpeed;
        public void SetMoveSpeed(float value) => _moveSpeed = value;
    }
}