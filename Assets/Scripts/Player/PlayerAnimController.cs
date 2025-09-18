using Platformer2D.GameManagers;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerAnimController : Singleton<PlayerAnimController>
    {
        [Header("VFXs Prefabs")]
        [SerializeField] private ParticleSystem _dustVFXPrefab;

        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private PlayerController _playerController;
        private PlayerHealthController _playerHealthController;

        private static readonly int IsMoving = Animator.StringToHash("moveSpeed");
        private static readonly int jumpSpeed = Animator.StringToHash("jumpSpeed");
        private static readonly int onGround = Animator.StringToHash("isGrounded");
        private static readonly int isDashing = Animator.StringToHash("isDashing");
        private static readonly int hurt = Animator.StringToHash("Hurt");

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerController = PlayerController.Instance;
            _playerHealthController = PlayerHealthController.Instance;
        }

        public void PlayerMoveAnimEvent(float speed) => _animator.SetFloat(IsMoving, Mathf.Abs(speed));

        public void PlayerJumpAnimEvent(float x) => _animator.SetFloat(jumpSpeed, x);

        public void PlayerLandAnimEvent(bool isFalling) => _animator.SetBool(onGround, isFalling);

        public void PlayerSprintAnimEvent(bool isSprinting) => _animator.SetBool(isDashing, isSprinting);

        public void PlayerHurtAnimEvent() => _animator.SetTrigger(hurt);

        public void PlayDustVFX()
        {
            if (_dustVFXPrefab == null) return;

            float destroyDelay = 0.5f;
            Vector3 offset = new Vector3((float)0.0324, (float)-0.8991, (float)0);
            ParticleSystem dust = Instantiate(_dustVFXPrefab, transform.position + offset, Quaternion.identity);

            dust.Play();
            Destroy(dust.gameObject, destroyDelay);
        }
    }
}
