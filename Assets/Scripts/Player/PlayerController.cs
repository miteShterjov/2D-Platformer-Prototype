using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer2D
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Player Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintMultiplier = 2f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private int _maxJumps = 2;

        private static readonly int IsMoving = Animator.StringToHash("moveSpeed");
        private static readonly int jumpSpeed = Animator.StringToHash("jumpSpeed");
        private static readonly int onGround = Animator.StringToHash("isGrounded");
        private static readonly int isDashing = Animator.StringToHash("isDashing");

        private InputSystem_Actions _playerInput;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private Vector2 _moveInput = Vector2.zero;
        private bool isGrounded;
        private float _currentJumpCount;
        private float _defaultMoveSpeed;

        protected override void Awake()
        {
            base.Awake();

            _playerInput = new InputSystem_Actions();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _playerInput.Player.Jump.performed += ctx => PlayerJump();
            _playerInput.Player.Sprint.performed += ctx => PlayerSprint();
            _playerInput.Player.Sprint.canceled += ctx => ResetMoveSpeed();

            _currentJumpCount = _maxJumps;
            _defaultMoveSpeed = _moveSpeed;
        }

        void Update()
        {
            _moveInput = _playerInput.Player.Move.ReadValue<Vector2>();

            // print(Keyboard.current.leftShiftKey.isPressed);
            

            // reset the available jumps when grounded
            if (IsGrounded() && _currentJumpCount != _maxJumps) _currentJumpCount = _maxJumps;
        }

        void FixedUpdate()
        {
            isGrounded = IsGrounded();
            PlayerMove();

            UpdatePlayerAnimEvents();
        }

        public void OnEnable()
        {
            _playerInput.Enable();
        }

        public void OnDisable()
        {
            _playerInput.Disable();
        }

        private void PlayerMove()
        {
            // Only control horizontal movement, let physics handle vertical
            _rigidbody2D.linearVelocity = new Vector2(_moveInput.x * _moveSpeed, _rigidbody2D.linearVelocity.y);

            // Flip the player sprite based on movement direction
            if (_moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (_moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        }

        private void PlayerJump()
        {
            if (_maxJumps > 0)
            {
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, _jumpForce);
                _currentJumpCount--;
            }
        }

        private void PlayerSprint()
        {
            if (Keyboard.current.leftShiftKey.isPressed) _moveSpeed *= _sprintMultiplier;
        }

        private void ResetMoveSpeed()
        {
            _moveSpeed = _defaultMoveSpeed;
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }

        private void UpdatePlayerAnimEvents()
        {
            _animator.SetFloat(IsMoving, Mathf.Abs(_moveInput.x));
            _animator.SetFloat(jumpSpeed, _rigidbody2D.linearVelocity.y);
            _animator.SetBool(onGround, isGrounded);
            _animator.SetBool(isDashing, Keyboard.current.leftShiftKey.isPressed);

        }

        private void OnDrawGizmos()
        {
            if (_groundCheck == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
