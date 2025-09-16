using UnityEngine;

namespace Platformer2D.Misc.Parallax
{
    public class ParallaxBackground : MonoBehaviour
    {
        public ParallaxCamera parallaxCamera;
        System.Collections.Generic.List<ParallaxLayer> parallaxLayers = new System.Collections.Generic.List<ParallaxLayer>();

        void Start()
        {
            if (parallaxCamera == null)
                parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

            if (parallaxCamera != null)
                parallaxCamera.onCameraTranslate += Move;

            SetLayers();
        }

        void SetLayers()
        {
            parallaxLayers.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

                if (layer != null)
                {
                    layer.name = "Layer-" + i;
                    parallaxLayers.Add(layer);
                }
            }
        }

        void Move(float delta)
        {
            foreach (ParallaxLayer layer in parallaxLayers)
            {
                layer.Move(delta);
            }
        }
    }
}