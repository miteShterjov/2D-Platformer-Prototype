using UnityEngine;
using UnityEngine.UI;

namespace Platformer2D.Misc
{
    public class Fader : MonoBehaviour
    {
        Image fader;
        Color faderColor;

        void Start()
        {
            fader = GetComponent<Image>();
            faderColor = fader.color;
        }

        public void FadeToBlack(float duration = 1f)
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                faderColor.a = 1 - (duration / 1f);
                fader.color = faderColor;
            }
        }

        public void FadeFromBlack(float duration = 1f)
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                faderColor.a = duration / 1f;
                fader.color = faderColor;
            }
        }
    }
}
