using UnityEngine;

namespace Platformer2D.GameMech.PickUps
{
    public interface IPickUps
    {
        public void OnPickUpApplyEffect(GameObject player);
        public void DestroyGameObject();
    }
}
