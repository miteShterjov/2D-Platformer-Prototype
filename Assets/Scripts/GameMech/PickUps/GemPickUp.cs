using Platformer2D.Player;
using UnityEngine;

namespace Platformer2D.GameMech.PickUps
{
    public class GemPickUp : MonoBehaviour, IPickUps
    {
        public void OnPickUpApplyEffect(GameObject player)
        {
            player.GetComponent<PlayerStats>().GetRichQuick();
            GetComponent<Animator>().SetTrigger("PickedUp");
        }
        public void DestroyGameObject() => Destroy(gameObject);
    }
}
