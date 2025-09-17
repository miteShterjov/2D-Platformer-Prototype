using UnityEngine;

namespace Platformer2D.Enemy
{
    public class DamageDealer : MonoBehaviour
    {
        public float DamageAmmount { get => damageAmmount; set => damageAmmount = value; }
        [SerializeField] private float damageAmmount = 5f;
    }
}
