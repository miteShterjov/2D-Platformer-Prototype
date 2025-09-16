using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer2D
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Move Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintMultiplier = 2f;
        [SerializeField] private float _sprintStaminaCostPerSecond = 10f;
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private int _maxJumps = 2;
        [SerializeField] private float _jumpStaminaCost = 2f;
        [Header("Gorund Checker Settings")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [Header("VFXs Prefabs")]
        [SerializeField] private ParticleSystem _dustVFXPrefab;

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
        private int _lastDirection = 1; // 1 for right, -1 for left

        private bool _wasGrounded; // Add this field

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

            // spent stamina while sprinting
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                Player.Instance.UpdateStamina(-Time.deltaTime * _sprintStaminaCostPerSecond);
            }
            // reset the available jumps when grounded
            if (IsGrounded() && _currentJumpCount != _maxJumps) _currentJumpCount = _maxJumps;
        }

        void FixedUpdate()
        {
            isGrounded = IsGrounded();

            // Play dust VFX when landing
            if (isGrounded && !_wasGrounded)
            {
                PlayDustVFX();
            }
            _wasGrounded = isGrounded;
            
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
            _rigidbody2D.linearVelocity = new Vector2(_moveInput.x * _moveSpeed, _rigidbody2D.linearVelocity.y);

            int currentDirection = _moveInput.x > 0 ? 1 : (_moveInput.x < 0 ? -1 : _lastDirection);

            // Only play dust VFX when direction changes
            if (_moveInput.x != 0 && isGrounded && currentDirection != _lastDirection)
            {
                PlayDustVFX();
                _lastDirection = currentDirection;
            }

            // Flip the player sprite based on movement direction
            if (_moveInput.x > 0) FlipPlayerSprite(1);
            else if (_moveInput.x < 0) FlipPlayerSprite(-1);
        }

        private void FlipPlayerSprite(float x)
        {
            transform.localScale = new Vector3(x, 1, 1);
        }

        private void PlayerJump()
        {
            if (Player.Instance.CurrentStamina == 0) return;

            if (_maxJumps > 0)
            {
                PlayDustVFX();
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, _jumpForce);
                Player.Instance.UpdateStamina(-_jumpStaminaCost);
                _currentJumpCount--;

            }
        }

        private void PlayerSprint()
        {
            if (Player.Instance.CurrentStamina == 0) return;
            if (Keyboard.current.leftShiftKey.isPressed) _moveSpeed *= _sprintMultiplier;
            PlayDustVFX();
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

        private void PlayDustVFX()
        {
            if (_dustVFXPrefab == null) return;

            float destroyDelay = 0.5f;
            Vector3 offset = new Vector3((float)0.0324, (float)-0.8991, (float)0);
            ParticleSystem dust = Instantiate(_dustVFXPrefab, transform.position + offset, Quaternion.identity);

            dust.Play();
            Destroy(dust.gameObject, destroyDelay); 
        }

        private void OnDrawGizmos()
        {
            if (_groundCheck == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
