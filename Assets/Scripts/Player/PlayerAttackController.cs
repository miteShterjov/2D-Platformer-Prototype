using Platformer2D.GameManagers;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerAttackController : Singleton<PlayerAttackController>
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float magikaCost = 10f;

        private float attackTimer = 0f;

        void Update()
        {
            if (PlayerController.Instance.IsFlipedX())
            {
                shootPoint.localPosition = new Vector2(-Mathf.Abs(shootPoint.localPosition.x), shootPoint.localPosition.y);
            }
            else
            {
                shootPoint.localPosition = new Vector2(Mathf.Abs(shootPoint.localPosition.x), shootPoint.localPosition.y);
            }

            if (attackTimer < attackCooldown)
            {
                attackTimer += Time.deltaTime;
            }
        }

        public void ShootFireball()
        {
            if (attackTimer < attackCooldown) return;
            if (PlayerStats.Instance.CurrentMagika < magikaCost) return;
            Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
            PlayerStats.Instance.SpendMagika(magikaCost);
            attackTimer = 0f;
        }
    }

}
