using System;
using UnityEngine;

namespace Platformer2D
{
    public class Player : Singleton<Player>
    {
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public float CurrentStamina { get => currentStamina; set => currentStamina = value; }
        public float CurrentMagika { get => currentMagika; set => currentMagika = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float MaxStamina { get => maxStamina; set => maxStamina = value; }
        public float MaxMagika { get => maxMagika; set => maxMagika = value; }

        [Header("Basic Stats")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float maxMagika = 100f;
        [Header("Regen Rates (per second)")]
        [SerializeField] private float healthRegenRate = 1f;
        [SerializeField] private float staminaRegenRate = 5f;
        [SerializeField] private float magikaRegenRate = 3f;

        private float currentHealth;
        private float currentStamina;
        private float currentMagika;

        void Start()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            CurrentMagika = MaxMagika;
        }

        void Update()
        {
            RegenerateResources();
        }

        public void ResetAllResources()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            CurrentMagika = MaxMagika;
        }

        public void ResetResource(string resourceType)
        {
            switch (resourceType.ToLower())
            {
                case "health":
                    CurrentHealth = MaxHealth;
                    break;
                case "stamina":
                    CurrentStamina = MaxStamina;
                    break;
                case "magika":
                    CurrentMagika = MaxMagika;
                    break;
                default:
                    Debug.LogWarning("Invalid resource type");
                    break;
            }
        }

        public void UpdateHealth(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }

        public void UpdateStamina(float amount)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, MaxStamina);
        }

        public void UpdateMagika(float amount)
        {
            CurrentMagika = Mathf.Clamp(CurrentMagika + amount, 0, MaxMagika);
        }

        public void SpendStamina(float amount)
        {
            if (CurrentStamina <= 0) return;

            if (amount <= CurrentStamina) UpdateStamina(-amount);
        }

        public void SpendMagika(float amount)
        {
            if (CurrentMagika <= 0) return;

            if (amount <= CurrentMagika) UpdateMagika(-amount);
        }

        private void RegenerateResources()
        {
            if (CurrentHealth < MaxHealth)
            {
                UpdateHealth(healthRegenRate * Time.deltaTime);
            }

            if (CurrentStamina < MaxStamina)
            {
                UpdateStamina(staminaRegenRate * Time.deltaTime);
            }

            if (CurrentMagika < MaxMagika)
            {
                UpdateMagika(magikaRegenRate * Time.deltaTime);
            }
        }

    }
}
