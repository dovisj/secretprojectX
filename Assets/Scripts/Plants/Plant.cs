using System.Collections;
using System.Collections.Generic;
using Managers;
using MEC;
using Misc;
using Store;
using UnityEngine;
using UnityEngine.UI;

namespace Plants
{
    [RequireComponent(typeof(StoreItem))]
    public class Plant : MonoBehaviour
    {
        [SerializeField]
        private PlantData _plantData;

        private ParticleSystem _particleSystem;
        private SpriteRenderer mainSpriteRenderer;
        [SerializeField] private float brancSizeVariations = 0.2f;
        [SerializeField] private float branchPlacementRadius = 1f;
        [SerializeField] private int maxMutationCount = 3;

        [SerializeField]
        private Slider waterSlider;
        [SerializeField]
        private Slider growthSlider;
        private string plantName;
        private int price;
        private List<Mutation> mutationEffects;
        private float spawnChance;
        private float delayBetweenStages;
        private int waterNeeds;
        public float currentWater=100;
        private int fertilizerNeeds;
        private int maxGrowthStages;
        private int currentGrowthStage;
        private float growthPercentage;
        [SerializeField]
        private AnimationCurve branchAnimCurve;

        private Color32 branchColor;
        [SerializeField] private float branchAnimSpeed;

        public int CurrentGrowthStage
        {
            get => currentGrowthStage;
            private set => currentGrowthStage = Mathf.Clamp(value, 0, maxGrowthStages);
        }


        private bool _hasGrown;
        private bool _isPlaced;

        public bool IsPlaced
        {
            set
            {
                if (value)
                {
                    _plantPlacedDelegate.Invoke();
                }

                _isPlaced = value;
            }

            get => _isPlaced;
        }

        private delegate void OnPlantPlacedDelegate();

        private OnPlantPlacedDelegate _plantPlacedDelegate;

        public bool HasGrown
        {
            get => _hasGrown;
            set
            {
                _hasGrown = value;
                _particleSystem.Play();
            }
        }

        public PlantData PlantData
        {
            get => _plantData;
            set
            {
                _plantData = value;
                SetData(_plantData);
            }
        }

        public float GrowthPercentage
        {
            get => growthPercentage;
            set => growthPercentage = Mathf.Clamp(value,0,100);
        }

        public int Price => price;

        void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            waterSlider.value = currentWater;
            growthSlider.value = growthPercentage;
            waterSlider.gameObject.SetActive(false);
            growthSlider.gameObject.SetActive(false);
            SetData(_plantData);
            mainSpriteRenderer = GetComponent<SpriteRenderer>();
            mainSpriteRenderer.sprite = PlantManager.Instance.GetRandomSeedBag();
            branchColor = PlantManager.Instance.GetRandomBranchColor();
            mainSpriteRenderer.color = branchColor;
            _plantPlacedDelegate += StartGrowingPlant;
         
        }
        
        private void SetData(PlantData data)
        {
            plantName = data.plantName;
            price = data.price;
            spawnChance = data.spawnChance;
            delayBetweenStages = data.delayBetweenStages;
            waterNeeds = data.waterNeeds;
            fertilizerNeeds = data.fertilizerNeeds;
            maxGrowthStages = data.maxGrowthStages;
        }

        public void PlacePlant(Vector3 position, Quaternion rotation)
        {
            transform.rotation = rotation;
            transform.position = position;
            _isPlaced = true;
            _plantPlacedDelegate.Invoke();
        }

        void StartGrowingPlant()
        {
            growthSlider.gameObject.SetActive(true);
            waterSlider.gameObject.SetActive(true);
            Timing.RunCoroutine(GrowPlant(),Segment.SlowUpdate);
        }

        IEnumerator<float> GrowPlant()
        {
            mainSpriteRenderer.sprite = _plantData.plantSprite.initialFrame;
            while (true)
            {
                waterSlider.value = currentWater;
                growthSlider.value = growthPercentage;
                if (currentWater > 0)
                {
                    currentWater -= Time.deltaTime*_plantData.waterNeeds;
                    if (currentGrowthStage <= maxGrowthStages)
                    {
                        growthPercentage += Time.deltaTime * _plantData.growthSpeed;
                        if (33 - growthPercentage < 5 && currentGrowthStage == 0)
                        {
                            Debug.Log("Stage 1");
                          
                            GoUpAStage();
                        }
                        else if (66 - growthPercentage < 5 && currentGrowthStage == 1)
                        {
                            Debug.Log("Stage 2");
                            GoUpAStage();
                        }
                        else if (growthPercentage >= 100 && currentGrowthStage == 2)
                        {
                            Debug.Log("Stage 3");
                            GoUpAStage();
                        }
                    }
                }
                if (currentGrowthStage == maxGrowthStages)
                {
                    HasGrown = true;
                    Debug.Log("Plant has grown!",this);
                    growthSlider.gameObject.SetActive(false);
                    waterSlider.gameObject.SetActive(false);
                    break;
                }

                yield return 0f;
            }
        }

        private void GoUpAStage()
        {
            price = (int)(Price *  (1 + growthPercentage/100));
            currentGrowthStage++;
            if (currentGrowthStage != 3)
            {
                mainSpriteRenderer.sprite = _plantData.plantSprite.GetNextFrame(currentGrowthStage); 
            }
        }
        
        // void PlaceBranches()
        // {
        //     float angle = Random.Range(0, 10) * Mathf.PI * 2f / 10;
        //     Vector3 newPos = transform.position + new Vector3(0,3f) + new Vector3(Mathf.Cos(angle) * branchPlacementRadius, Mathf.Sin(angle) * branchPlacementRadius);
        //     GameObject branch = Instantiate(PlantManager.Instance.GetRandomBranch(), newPos,
        //         Quaternion.identity,transform);
        //     branch.transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
        //     float currX = branch.transform.localScale.x;
        //     float currY = branch.transform.localScale.y;
        //     float randXmod = 1 + Random.Range(0, brancSizeVariations);
        //     float randYmod = 1 + Random.Range(0, brancSizeVariations);
        //     branch.transform.localScale = new Vector3(currX * randXmod, currY * randYmod);
        //     Vector3 endSize = branch.transform.localScale;
        //     branch.transform.localScale = Vector3.zero;
        //     branch.GetComponent<SpriteRenderer>().color = branchColor;
        //     StartCoroutine(AnimateGrowth(branch, endSize));
        // }

        IEnumerator AnimateGrowth(GameObject branch,Vector3 endSize)
        {
            float t = 0;
            while (t <= 1.0f) {
                t += Time.deltaTime*branchAnimSpeed; // Goes from 0 to 1, incrementing by step each time
                branch.transform.localScale = Vector3.Lerp(Vector3.zero, endSize, branchAnimCurve.Evaluate(t)); // Move objectToMove closer to b
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        public void KillPlant()
        {
            mainSpriteRenderer.sprite = _plantData.deadPlant;
            price = 0;
            _hasGrown = true;
            growthSlider.gameObject.SetActive(false);
            waterSlider.gameObject.SetActive(false);
        }
    }
}