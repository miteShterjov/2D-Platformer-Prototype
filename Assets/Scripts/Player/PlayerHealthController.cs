using System.Collections;
using Platformer2D.Enemy;
using Platformer2D.GameManagers;
using Platformer2D.GameMech.PickUps;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerHealthController : Singleton<PlayerHealthController>
    {
        [SerializeField] private float invincibilityDuration = 1f;
        [SerializeField] private float knockbackDuration = 0.3f; // Duration to disable input
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private bool isInvincible = false;
        private float invincibilityTimer;
        

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            invincibilityTimer = invincibilityDuration;
        }

        void Update()
        {
            // handle invincibility timer
            if (isInvincible) invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                invincibilityTimer = invincibilityDuration;
            }
        }

        private float CurrentHealth
        {
            get => PlayerStats.Instance.CurrentHealth;
            set => PlayerStats.Instance.CurrentHealth = value;
        }

        private float MaxHealth => PlayerStats.Instance.MaxHealth;

        public void PlayerTakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                PlayerDies();
            }
        }

        public void PlayerGetsHealed(int healAmount)
        {
            CurrentHealth += healAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        private void PlayerDies()
        {
            PlayerStats.Instance.gameObject.SetActive(false);
            RespawnPlayer();

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<DamageDealer>() && !isInvincible)
            {
                PlayerTakeDamage(collision.GetComponent<DamageDealer>().DamageAmmount);
                DoPlayerTakeDamageSequence(collision.transform);
                isInvincible = true;
            }

            collision.GetComponent<IPickUps>()?.OnPickUpApplyEffect(gameObject);
        }

        private void DoPlayerTakeDamageSequence(Transform damageSource)
        {
            float alphaIndex = 0.6f;
            float alphaChangeDuration = 0.8f;

            PlayerAnimController.Instance.PlayerHurtAnimEvent();
            ModifySpriteAlpha(alphaIndex, alphaChangeDuration);
            KnockbackPlayer(damageSource.transform.position);
            DisablePlayerInput(knockbackDuration);
        }

        private void DisablePlayerInput(float duration)
        {
            PlayerController.Instance.enabled = false;
            Invoke(nameof(EnablePlayerInput), duration);
        }

        private void EnablePlayerInput()
        {
            PlayerController.Instance.enabled = true;
        }

        private void ModifySpriteAlpha(float index, float duration)
        {
            Color tempColor = spriteRenderer.color;
            tempColor.a = index;
            spriteRenderer.color = tempColor;
            Invoke(nameof(ResetSpriteAlpha), duration);
        }

        private void ResetSpriteAlpha()
        {
            Color tempColor = spriteRenderer.color;
            tempColor.a = 1f;
            spriteRenderer.color = tempColor;
        }

        private void KnockbackPlayer(
            Vector2 hitSourcePosition,
            float knockbackForce = 3f,
            float pushBackDistance = 2.5f
            )
        {
            if (rb == null) return;

            // Calculate direction: from hit source to player
            Vector2 knockDirection = ((Vector2)transform.position - hitSourcePosition).normalized;
            knockDirection.y = 0.5f; // Add a little upward force for effect
            knockDirection.Normalize();

            // Apply knockback force
            rb.linearVelocity = Vector2.zero; // Reset current velocity for consistent knockback
            rb.AddForce(knockDirection * knockbackForce, ForceMode2D.Impulse);

            // Push the player back instantly by a small distance
            Vector3 pushBack = (Vector3)(knockDirection * pushBackDistance);
            transform.position += pushBack;
        }


        private void RespawnPlayer()
        {
            FaderManager.Instance.FadeToBlack(0.6f);
            PlayerStats.Instance.ResetAllResources();
            PlayerStats.Instance.transform.position = PlayerStats.Instance.RespawnPoint;
            PlayerStats.Instance.gameObject.SetActive(true);
            FaderManager.Instance.FadeFromBlack(0.6f);
        }
    }
}
