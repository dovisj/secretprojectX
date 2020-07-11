using System.Collections;
using System.Collections.Generic;
using Managers;
using MEC;
using Misc;
using UnityEngine;

namespace Plants
{
    public class MutationEffectResize : Mutation
    {
        private Vector3 initalScale;
        private float initialSpeed;
        public float resizeFactor = 2;

        public MutationEffectResize()
        {
            uid = "RESIZE";
            EffectDescription = "I think this makes me grow!";
            Duration = 10f;
        }

        public override void Activate(GameObject caller)
        {
            base.Activate(caller);
            initalScale = caller.transform.localScale;
            Timing.RunCoroutine(AnimateGrowth(initalScale * resizeFactor));
        }
        
        public override void Reverse()
        {
            Timing.RunCoroutine(ReverseGrowth());
        }

        IEnumerator<float> AnimateGrowth(Vector3 endSize)
        {
            float t = 0;
            while (t <= 1.0f) {
                t += Timing.DeltaTime;
                Caller.transform.localScale = Vector3.Lerp(Caller.transform.localScale, endSize, t);
                yield return Timing.WaitForOneFrame;
            }
            yield return 0f;
        }
        IEnumerator<float> ReverseGrowth()
        {
            float t = 0;
            while (t <= 1.0f) {
                t += Timing.DeltaTime;
                Caller.transform.localScale = Vector3.Lerp(Caller.transform.localScale, initalScale, t); 
                yield return Timing.WaitForOneFrame;
            }
            yield return 0f;
        }
    }
}
