using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Plants;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private int availableCurrency = 1000;
    [SerializeField]
    private Dictionary<Guid,PlantData> currentStock;
    [SerializeField]
    private int maxStock = 5;

    private float stockChangeTimer;
    protected static StoreManager instance;

    public delegate void OnStockAdded(Guid guid, PlantData data);
    public OnStockAdded onStockAdded;

    public static StoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (StoreManager)FindObjectOfType(typeof(StoreManager));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(StoreManager) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        currentStock = new Dictionary<Guid, PlantData>();
    }

    private void Start()
    {
        GenerateStock();
    }

    void GenerateStock()
    {
        for (int i = 0; i < maxStock; i++)
        {
            Guid newGuid = Guid.NewGuid();
            PlantData data = PlantManager.Instance.GetRandomPlantType();
            currentStock.Add(newGuid,data);
            onStockAdded.Invoke(newGuid,data);
        }
     
    }

    public void Buy(Guid guid)
    {
        PlantData plantData;
        if (currentStock.TryGetValue(guid, out plantData))
        {
            Debug.Log("Bought:: "+plantData.plantName+" for:: "+plantData.price);
            Plant plant = Instantiate(PlantManager.Instance.plantPrefab);
            plant.PlantData = plantData;
        }
    }

    void Sell(Plant plant)
    {
        
    }
}
