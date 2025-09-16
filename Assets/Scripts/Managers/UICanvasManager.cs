using UnityEngine;
using UnityEngine.UI;

namespace Platformer2D
{
    public class UICanvasManager : MonoBehaviour
    {
        [Header("Player Resource Bars")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider magikaBar;

        void Start()
        {
            healthBar.maxValue = Player.Instance.MaxHealth;
            staminaBar.maxValue = Player.Instance.MaxStamina;
            magikaBar.maxValue = Player.Instance.MaxMagika;
        }

        void Update()
        {
            UpdateSliderInfo();
        }

        private void UpdateSliderInfo()
        {
            healthBar.value = Player.Instance.CurrentHealth;
            staminaBar.value = Player.Instance.CurrentStamina;
            magikaBar.value = Player.Instance.CurrentMagika;
        }
    }
}
