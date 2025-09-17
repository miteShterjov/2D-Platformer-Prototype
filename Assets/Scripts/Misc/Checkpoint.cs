using System;
using UnityEngine;

namespace Platformer2D.Misc
{
    public class Checkpoint : MonoBehaviour
    {
        public event EventHandler OnCheckpointActivated;

        [Header("Checkpoint Settings")]
        [SerializeField] private Sprite checkOn;
        [SerializeField] private Sprite checkOff;

        private bool isRespawnPoint = false;
        private bool wasRespawnPoint = false;
        private SpriteRenderer spriteRenderer;
        private Vector3 respawnPoint;

        public Vector3 RespawnPoint { get => respawnPoint; set => respawnPoint = value; }

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            RespawnPoint = transform.position;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!isRespawnPoint && !wasRespawnPoint) ActivateCheckpoint();
            }
        }
        // this method is used to reset last checkpoint
        public void ResetCheckpoint()
        {
            isRespawnPoint = false;
            wasRespawnPoint = true;
            spriteRenderer.sprite = checkOff;
        }
        // this method is used to set the new checkpoint
        private void ActivateCheckpoint()
        {
            isRespawnPoint = true;
            spriteRenderer.sprite = checkOn;
            OnCheckpointActivated?.Invoke(this, EventArgs.Empty);
        }

    }
}
