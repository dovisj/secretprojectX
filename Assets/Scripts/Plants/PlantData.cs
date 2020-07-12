using System.Collections.Generic;
using Managers;
using Misc;
using UnityEngine;
namespace Plants
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/New Plant")]
    public class PlantData : ScriptableObject
    {
        public string plantName = "Xuzerof Leaf";
        public string description = "This is an awesome plant";
        public int price = 10;
        public List<Mutation> mutationEffects;
        public float spawnChance = 1;
        public float delayBetweenStages = 0.01f;
        public int waterNeeds = 1;
        public int fertilizerNeeds;
        public int maxGrowthStages = 3;
        public PlantSprite plantSprite;
        public Sprite deadPlant;
        public Sprite seedSprite;
        public float growthSpeed=1;

        public void Randomize()
        {
            mutationEffects = new List<Mutation>();
            List<Mutation> randomMutations = PlantManager.Instance.GetRandomMutations(Random.Range(0,10));
            if (randomMutations.Count > 0)
            {
                mutationEffects.AddRange(randomMutations);
                GenerateDescription();
            }
            else
            {
                description = "";
            }
         
        }
        
        public void GenerateDescription()
        {
            description = "";
            foreach (Mutation mutationEffect in mutationEffects)
            {
                if (PlantManager.Instance.knownMutations.ContainsKey(mutationEffect.uid))
                {
                    description += "\n - "+mutationEffect.EffectDescription;
                }
                else
                {
                    description += "\n - unknown effect";
                }
            }
        }
    }
}