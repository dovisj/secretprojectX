using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plants;
using Store;
using UnityEngine;

namespace Managers
{
    public class StoreManager : MonoBehaviour
    {
        public int AvailableCurrency { get; private set; } = 1000;

        [SerializeField]
        private Dictionary<Guid,StoreItem> currentStock;
        [SerializeField]
        private int maxStock = 4;
        [SerializeField]
        private float maxStockTimer = 120;
        private float stockChangeTimer;
        
        protected static StoreManager instance;

        public delegate void OnStockAdded(Guid guid, StoreItem data);
        public OnStockAdded onStockAdded;
        public delegate void OnStockRemoved(Guid guid);
        public OnStockRemoved onStockRemoved;
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
            stockChangeTimer = maxStockTimer;
        }

        private void Start()
        {
            GenerateStock();
            StartCoroutine(RestockTimer());
        }
        IEnumerator RestockTimer()
         {
             while (true)
             {
                 stockChangeTimer -= 1;
                 if (stockChangeTimer <= 0)
                 {
                     stockChangeTimer = maxStockTimer;
                     GenerateStock();
                 }
                 UIManager.Instance.SetStoreTimer(Timers.Countdown(stockChangeTimer));
                 yield return new WaitForSeconds(1f);
             }
      
         }

        void GenerateStock()
        {
            if (currentStock.Count > 0)
            {
                var itemsToRemove = currentStock.ToArray();
                foreach (var item in itemsToRemove)
                {
                    currentStock.Remove(item.Key);
                    onStockRemoved.Invoke(item.Key);
                }
            }
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

        private void RemoveItem(Guid guid)
        {
            if (currentStock.ContainsKey(guid))
            {
                currentStock.Remove(guid);
            }
            
        }

        private StoreItem GetRandomStoreItem()
        {
            PlantData plant = PlantManager.Instance.GetRandomPlantType();
            plant.Randomize();
            return new StoreItem(plant);
        }
    }
}
