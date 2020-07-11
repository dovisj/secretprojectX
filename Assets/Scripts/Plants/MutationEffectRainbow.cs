using System.Collections;
using System.Collections.Generic;
using Managers;
using MEC;
using Misc;
using UnityEngine;

namespace Plants
{
    public class MutationEffectRainbow : Mutation
    {
        private List<Color> initalColors;
        private Queue<Color> randomColors;
        private List<SpriteRenderer> _renderers;

        public MutationEffectRainbow()
        {
            uid = "RAINBOW";
            EffectDescription = "Double rainbow all the way!";
            Duration = 30f;
        }

        public override void Activate(GameObject caller)
        {
            randomColors = new Queue<Color>();
            initalColors = new List<Color>();
            for (int i = 0; i < 10; i++)
            {
                randomColors.Enqueue(new Color(Random.value, Random.value, Random.value));
            }
            _renderers = new List<SpriteRenderer>(caller.GetComponentsInChildren<SpriteRenderer>());
            base.Activate(caller);
            if (_renderers.Count > 0)
            {
                foreach (var renderer in _renderers)
                {
                    initalColors.Add(renderer.color);
                }

                Timing.RunCoroutine(Colorize());
            }
        }
        
        public override void Reverse()
        {
            Timing.RunCoroutine(ReverseColor());
        }

        IEnumerator<float> Colorize()
        {
            float t = 0;
            Color currCol = randomColors.Dequeue();
            while (t <= 1.0f && randomColors.Count > 0) {
                t += Timing.DeltaTime;
                foreach (SpriteRenderer spriteRenderer in _renderers)
                {
                    spriteRenderer.color= Color.Lerp(spriteRenderer.color,currCol , t);
                }
                if (t > 0.9f && randomColors.Count > 0)
                {
                    t = 0;
                    currCol = randomColors.Dequeue();
                }
                yield return Timing.WaitForOneFrame;
            }
            yield return 0f;
        }
        IEnumerator<float> ReverseColor()
        {
            float t = 0;
            int i = 0;
            while (t <= 1.0f) {
                t += Timing.DeltaTime;
                foreach (SpriteRenderer spriteRenderer in _renderers)
                {
                    spriteRenderer.color= Color.Lerp(spriteRenderer.color, initalColors[i], t);
                    i++;
                }

                i = 0;
                yield return Timing.WaitForOneFrame;
            }
            yield return 0f;
        }
    }
}
