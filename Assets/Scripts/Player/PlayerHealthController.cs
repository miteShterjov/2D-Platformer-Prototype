using System;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        private int CurrentHealth
        {
            get => Player.Instance.CurrentHealth;
            set => Player.Instance.CurrentHealth = value;
        }

        private int MaxHealth => Player.Instance.MaxHealth;

        public void PlayerTakeDamage(int damage)
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
            Player.Instance.gameObject.SetActive(false);
            print("Player Died!");
        }
    }
}
