using Platformer2D.Player;
using UnityEngine;

namespace Platformer2D.GameMech.PickUps
{
    public class AcornStamina : MonoBehaviour, IPickUps
    {
        [SerializeField] private int staminaAmount = 20;

        public void OnPickUpApplyEffect(GameObject player)
        {
            var playerStats = player.GetComponent<PlayerStats>();
            if (playerStats.CurrentStamina == playerStats.MaxStamina) return;
            playerStats.UpdateStamina(staminaAmount);
            GetComponent<Animator>().SetTrigger("PickedUp");
        }

        public void DestroyGameObject() => Destroy(gameObject);
    }
}
