using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Plants;
using Store;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public int AvailableCurrency { get; private set; } = 1000;

    [SerializeField]
    private Dictionary<Guid,StoreItem> currentStock;
    [SerializeField]
    private int maxStock = 5;

    private float stockChangeTimer;
    protected static StoreManager instance;

    public delegate void OnStockAdded(Guid guid, StoreItem data);
    public OnStockAdded onStockAdded;
    
    public delegate void OnItemBought(Guid guid, StoreItem storeItem);
    public OnItemBought onItemBought;

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
        currentStock = new Dictionary<Guid, StoreItem>();
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
            StoreItem storeItem = GetRandomStoreItem();
            currentStock.Add(newGuid,storeItem);
            onStockAdded.Invoke(newGuid,storeItem);
        }
     
    }

    public void Buy(Guid guid)
    {
        StoreItem storeItem;
        if (!currentStock.TryGetValue(guid, out storeItem)) return;
        if (AvailableCurrency - storeItem.price < 0)
        {
            Debug.Log("STORE_NOT_ENOUGH_CASH");
            EventManager.TriggerEvent("STORE_NOT_ENOUGH_CASH");
        }
        else
        {
            AvailableCurrency -= storeItem.price;
            Debug.Log("Bought:: "+storeItem.itemName+" for:: "+storeItem.price);
            onItemBought?.Invoke(guid,storeItem);
        }
    }

    void Sell(Plant plant)
    {
        
    }

    private StoreItem GetRandomStoreItem()
    {
        return new StoreItem(PlantManager.Instance.GetRandomPlantType());
    }
}
