using System.Collections.Generic;
using MEC;
using Misc;
using Plants;
using UnityEngine;

namespace Player
{
    public class PlayerMutations : MonoBehaviour
    {
        private Queue<Mutation> queuedMutations;
        void Awake()
        {
            queuedMutations = new Queue<Mutation>();
            Timing.RunCoroutine(CheckMutations(), Segment.SlowUpdate);
            AddMutation(new MutationEffectResize());
        }


        public void AddMutation(Mutation mutation)
        {
            queuedMutations.Enqueue(mutation);
        }

        private IEnumerator<float> CheckMutations()
        {
            while (true)
            {
                if (queuedMutations.Count > 0)
                {
                    Mutation mutation = queuedMutations.Dequeue();
                    mutation.Activate(gameObject);
                }
                yield return Timing.WaitForSeconds(1f);
            }
         
        }
    }
}
