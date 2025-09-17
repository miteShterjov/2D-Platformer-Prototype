using UnityEngine;
using UnityEngine.UI;
using Platformer2D.Player;

namespace Platformer2D.GameManagers
{
    public class UICanvasManager : Singleton<UICanvasManager>
    {
        [Header("Player Resource Bars")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider magikaBar;

        void Start()
        {
            healthBar.maxValue = PlayerStats.Instance.MaxHealth;
            staminaBar.maxValue = PlayerStats.Instance.MaxStamina;
            magikaBar.maxValue = PlayerStats.Instance.MaxMagika;
        }

        void Update()
        {
            UpdateSliderInfo();
        }

        private void UpdateSliderInfo()
        {
            healthBar.value = PlayerStats.Instance.CurrentHealth;
            staminaBar.value = PlayerStats.Instance.CurrentStamina;
            magikaBar.value = PlayerStats.Instance.CurrentMagika;
        }
    }
}
