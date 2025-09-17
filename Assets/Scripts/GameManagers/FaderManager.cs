using Platformer2D.Misc;
using UnityEngine;

namespace Platformer2D.GameManagers
{
    public class FaderManager : Singleton<FaderManager>
    {
        [SerializeField] private Fader fader;

        public void FadeToBlack(float duration = 1f)
        {
            fader.FadeToBlack(duration);
        }

        public void FadeFromBlack(float duration = 1f)
        {
            fader.FadeFromBlack(duration);
        }
    }
}