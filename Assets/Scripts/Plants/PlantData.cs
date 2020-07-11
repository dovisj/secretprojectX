using Misc;
using UnityEngine;

namespace Plants
{
    [CreateAssetMenu(fileName = "Game", menuName = "Game/New Plant")]
    public class PlantData : ScriptableObject
    {
        public string plantName = "Xuzerof Leaf";
        public int price = 10;
        public MutationEffect[] mutationEffects;
        public float spawnChance = 1;
        public float delayBetweenStages = 0.01f;
        public int waterNeeds = 0;
        public int fertilizerNeeds;
        public int maxGrowthStages = 3;
    }
}