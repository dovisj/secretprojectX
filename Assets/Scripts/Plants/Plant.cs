using System.Collections;
using System.Collections.Generic;
using Managers;
using Misc;
using UnityEngine;

namespace Plants
{
    public class Plant : MonoBehaviour
    {
        [SerializeField]
        private PlantData _plantData;
        [SerializeField] private float brancSizeVariations = 0.2f;
        [SerializeField] private float branchPlacementRadius = 1f;
        private string plantName;
        private int price;
        private MutationEffect[] mutationEffects;
        private float spawnChance;
        private float delayBetweenStages;
        private int waterNeeds;
        private int fertilizerNeeds;
        private int maxGrowthStages;
        private int currentGrowthStage;
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
            set => _hasGrown = value;
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

        void Awake()
        {
            SetData(_plantData);
            branchColor = PlantManager.Instance.GetRandomBranchColor();
            _plantPlacedDelegate += StartGrowingPlant;
        }

        private void SetData(PlantData data)
        {
            plantName = data.plantName;
            price = data.price;
            MutationEffect[] mutationEffects = data.mutationEffects;
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
            StartCoroutine(GrowPlant());
        }

        IEnumerator GrowPlant()
        {
            while (true)
            {
                GoUpAStage();
                if (currentGrowthStage == maxGrowthStages)
                {
                    HasGrown = true;
                    break;
                }

                yield return new WaitForSeconds(delayBetweenStages);
            }
        }

        void GoUpAStage()
        {
            if (CurrentGrowthStage == 1)
            {
                GameObject stem = Instantiate(PlantManager.Instance.GetRandomStem(), transform.position, Quaternion.identity, transform);
                Vector3 endSize = stem.transform.localScale;
                StartCoroutine(AnimateGrowth(stem, endSize));
            }
            else if(CurrentGrowthStage > 1)
            {
                PlaceBranches();
            }

            CurrentGrowthStage++;
        }

        void PlaceBranches()
        {
            float angle = Random.Range(0, 10) * Mathf.PI * 2f / 10;
            Vector3 newPos = transform.position + new Vector3(0,3f) + new Vector3(Mathf.Cos(angle) * branchPlacementRadius, Mathf.Sin(angle) * branchPlacementRadius);
            GameObject branch = Instantiate(PlantManager.Instance.GetRandomBranch(), newPos,
                Quaternion.identity,transform);
            branch.transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
            float currX = branch.transform.localScale.x;
            float currY = branch.transform.localScale.y;
            float randXmod = 1 + Random.Range(0, brancSizeVariations);
            float randYmod = 1 + Random.Range(0, brancSizeVariations);
            branch.transform.localScale = new Vector3(currX * randXmod, currY * randYmod);
            Vector3 endSize = branch.transform.localScale;
            branch.transform.localScale = Vector3.zero;
            branch.GetComponent<SpriteRenderer>().color = branchColor;
            StartCoroutine(AnimateGrowth(branch, endSize));
        }

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
    }
}