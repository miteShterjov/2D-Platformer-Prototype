using System;
using Platformer2D.GameManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer2D.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        public bool IsLeft { get => isLeft; set => isLeft = value; }
        public bool IsGrounded1 { get => isGrounded; set => isGrounded = value; }
        public Vector2 MoveInput { get => _moveInput; set => _moveInput = value; }
        
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

        private InputSystem_Actions _playerInput;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer spriteRenderer;
        private Vector2 _moveInput = Vector2.zero;
        private bool isGrounded;
        private float _currentJumpCount;
        private float _defaultMoveSpeed;
        private int _lastDirection = 1; // 1 for right, -1 for left
        private bool _wasGrounded; // Add this field
        private bool isLeft;
        private bool isSprinting;

        private void PlayDustVFX() => PlayerAnimController.Instance.PlayDustVFX();


        protected override void Awake()
        {
            base.Awake();

            _playerInput = new InputSystem_Actions();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            _playerInput.Player.Jump.performed += ctx => PlayerJump();
            _playerInput.Player.Sprint.performed += ctx => PlayerSprint();
            _playerInput.Player.Sprint.canceled += ctx => ResetMoveSpeed();
            _playerInput.Player.Attack.performed += ctx => PlayerAttack();

            _currentJumpCount = _maxJumps;
            _defaultMoveSpeed = _moveSpeed;
        }

        void Update()
        {
            MoveInput = _playerInput.Player.Move.ReadValue<Vector2>();

            // spent stamina while sprinting
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                PlayerStats.Instance.UpdateStamina(-Time.deltaTime * _sprintStaminaCostPerSecond);
            }
            // reset the available jumps when grounded
            if (IsGrounded() && _currentJumpCount != _maxJumps) _currentJumpCount = _maxJumps;

            print(_moveInput.x);
        }

        void FixedUpdate()
        {
            IsGrounded1 = IsGrounded();

            // Play dust VFX when landing
            if (IsGrounded1 && !_wasGrounded) PlayDustVFX();
            _wasGrounded = IsGrounded1;

            PlayerMove();

            if (Keyboard.current.leftShiftKey.isPressed) isSprinting = true;
            else isSprinting = false;

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
            _rigidbody2D.linearVelocity = new Vector2(MoveInput.x * _moveSpeed, _rigidbody2D.linearVelocity.y);

            int currentDirection = MoveInput.x > 0 ? 1 : (MoveInput.x < 0 ? -1 : _lastDirection);

            // Only play dust VFX when direction changes
            if (MoveInput.x != 0 && IsGrounded1 && currentDirection != _lastDirection)
            {
                PlayDustVFX();
                _lastDirection = currentDirection;
            }

            // Flip the player sprite based on movement direction
            if (MoveInput.x > 0) FlipPlayerSprite(false);
            else if (MoveInput.x < 0) FlipPlayerSprite(true);
        }


        private void PlayerJump()
        {
            if (PlayerStats.Instance.CurrentStamina == 0) return;

            if (_maxJumps > 0)
            {
                PlayDustVFX();
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, _jumpForce);
                PlayerStats.Instance.UpdateStamina(-_jumpStaminaCost);
                _currentJumpCount--;

            }
        }

        private void PlayerSprint()
        {
            if (PlayerStats.Instance.CurrentStamina == 0) return;
            if (isSprinting) _moveSpeed *= _sprintMultiplier;
            PlayDustVFX();
        }

        private void PlayerAttack()
        {
            PlayerAttackController.Instance.ShootFireball();
        }

        private void UpdatePlayerAnimEvents()
        {
            PlayerAnimController.Instance.PlayerMoveAnimEvent(_moveInput.x);
            PlayerAnimController.Instance.PlayerJumpAnimEvent(_rigidbody2D.linearVelocity.y);
            PlayerAnimController.Instance.PlayerLandAnimEvent(isGrounded);
            PlayerAnimController.Instance.PlayerSprintAnimEvent(Keyboard.current.leftShiftKey.isPressed);
        }

        private void ResetMoveSpeed()
        {
            _moveSpeed = _defaultMoveSpeed;
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }

        private void FlipPlayerSprite(bool flip)
        {
            spriteRenderer.flipX = flip;
        }

        internal bool IsFlipedX()
        {
            return spriteRenderer.flipX;
        }

        private void OnDrawGizmos()
        {
            if (_groundCheck == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
