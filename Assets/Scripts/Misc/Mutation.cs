using System;
using System.Collections.Generic;
using Managers;
using MEC;
using UnityEngine;

namespace Misc
{
    public class Mutation
    {
        public string uid = "BASE";
        [Range(0,5f)]
        public float MovementSpeed;
        [Range(0.2f,1.5f)]
        public float ScaleFactor;
        [Range(3f,15f)]
        public float Duration = 5f;
        public string EffectDescription = "";
        
        public bool isKnown { get; private set; }
        public GameObject Caller;

        public virtual void Activate(GameObject caller)
        {
            if (!PlantManager.Instance.knownMutations.ContainsKey(uid))
            {
                PlantManager.Instance.knownMutations.Add(uid,this);
            }
            Caller = caller;
            isKnown = true;
            Timing.RunCoroutine(MutationTimer());
        }
        public virtual void Reverse()
        {
        }

        IEnumerator<float> MutationTimer()
        {
            while (true)
            {
                Duration -= Timing.DeltaTime;
                if (Duration <= 0)
                {
                    Reverse();
                }
                yield return Timing.WaitForOneFrame;
            }

            yield return 0f;
        }
    }
}
