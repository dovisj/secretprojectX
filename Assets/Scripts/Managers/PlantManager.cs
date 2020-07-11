using System;
using System.Collections.Generic;
using Plants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class PlantManager : MonoBehaviour
    {
        public LayerMask PlantPlotLayerMask;
        public Sprite[] seedBags;
        public GameObject[] branches;
        public GameObject[] stems;
        public GameObject[] extras;
        [SerializeField]
        private PlantData[] plantTypes;
        public Plant plantPrefab;
        public List<Color32> BranchColorPallete { get; private set; }
        
        protected static PlantManager instance;
        public static PlantManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (PlantManager)FindObjectOfType(typeof(PlantManager));

                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(PlantManager) + " is needed in the scene, but there is none.");
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            BranchColorPallete = new List<Color32>
            {
                new Color32(253, 197, 245,255),
                new Color32(179, 136, 235,255),
                new Color32(114, 221, 247,255),
                new Color32(243, 227, 124,255),
                new Color32(170, 250, 200,255),
                new Color32(255, 0, 0,255),
                new Color32(0, 0, 255,255),
                new Color32(0,255, 0,255)
            };
        }

        public PlantData GetRandomPlantType()
        {
            return plantTypes[Random.Range(0, plantTypes.Length)];
        }
        public Sprite GetRandomSeedBag()
        {
            return seedBags[Random.Range(0, seedBags.Length)];
        }

        public GameObject GetRandomStem()
        {
            return stems[Random.Range(0, stems.Length)];
        }
    
        public GameObject GetRandomBranch()
        {
            return branches[Random.Range(0, branches.Length)];
        }
    
        public GameObject GetRandomExtra()
        {
            return extras[Random.Range(0, extras.Length)];
        }
        
        public Color32 GetRandomBranchColor()
        {
            return BranchColorPallete[Random.Range(0, BranchColorPallete.Count)];
        }
    }
}
