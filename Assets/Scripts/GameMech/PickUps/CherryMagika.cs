using Platformer2D.Player;
using UnityEngine;

namespace Platformer2D.GameMech.PickUps
{
    public class CherryMagika : MonoBehaviour, IPickUps
    {
        [SerializeField] private int magikaAmount = 20;

        public void OnPickUpApplyEffect(GameObject player)
        {
            var playerStats = player.GetComponent<PlayerStats>();
            if (playerStats.CurrentMagika == playerStats.MaxMagika) return;
            playerStats.UpdateMagika(magikaAmount);
            GetComponent<Animator>().SetTrigger("PickedUp");
        }

        public void DestroyGameObject() => Destroy(gameObject);
    }
}
