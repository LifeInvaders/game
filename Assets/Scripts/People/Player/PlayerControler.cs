using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEditor;

namespace People.Player
{
    public class PlayerControler : MonoBehaviour
    {
        // Start is called before the first frame update
        private float _moveSpeed;
        [SerializeField] private float walkSpeed = 6;
        [SerializeField] private float runSpeed = 3;
        [SerializeField] private float jumpspeed = 5;

        private Vector2 _axis;


        private Rigidbody _rig;
        private Animator _anim;
        private CapsuleCollider _capsule;

        private bool _canRotate = true;
        private bool _canMove = true;
        private bool _canRun = true;

        public void SetMoveBool(bool state) => _canMove = state;
        public void SetCanRun(bool state) => _canRun = state;

        public void SetRotateBool(bool state) => _canRotate = state;

        void Start()
        {
            _axis = Vector2.zero;
            _rig = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
            _capsule = GetComponent<CapsuleCollider>();
            _moveSpeed = walkSpeed;
            if (PhotonNetwork.IsConnected && !gameObject.GetPhotonView().IsMine)
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

        public Vector2 GetAxis() => _axis;

        private void SetAnim()
        {
            _anim.SetFloat("Speed Front", Mathf.Abs(_axis.y) * _moveSpeed);
            // set les animations de marche latérale
            _anim.SetFloat("Speed Side", Mathf.Abs(_axis.x) * _moveSpeed);
            _anim.SetBool("Mirror", _axis.x < 0);
            _anim.SetBool("running", _moveSpeed > walkSpeed);
        }

        private bool IsGrounded() =>
            Physics.Raycast(_capsule.bounds.center, Vector3.down, _capsule.bounds.extents.y + 0.2f);

        private void Move()
        {
            SetAnim();
            Vector3 dir = (transform.right * _axis.x + transform.forward * _axis.y) * _moveSpeed;
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
            float time = 0;
            while (!IsGrounded())
            {
                _canMove = false;
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (time > 2.5f)
                    break;
            }

            _canMove = true;
        }

        void FixedUpdate()
        {
            if (_canMove)
                Move();
        }

        public void OnMovement(InputValue value) => _axis = value.Get<Vector2>();

        public void OnJump()
        {
            if (IsGrounded())
                Jump();
        }

        public void OnMenu(InputValue value)
        {
            if (value.isPressed) {/* Do stuff*/} 
            else throw new NotImplementedException();
            //TODO: Implement menu show/hide
        }

        public void OnRun(InputValue value) =>
            _moveSpeed = value.Get<float>().Equals(1) && _canRun ? runSpeed : walkSpeed;
    }
}