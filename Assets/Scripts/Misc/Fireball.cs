using UnityEngine;

namespace Platformer2D.Misc
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private int damage = 1;

        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            if (Player.PlayerController.Instance.IsLeft)
            {
                speed = -speed;
                spriteRenderer.flipX = true;
            }

            Destroy(gameObject, lifetime);
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
}
