
using System;
using System.Collections.Generic;
using Plants;
using UIElements;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    protected static UIManager instance;
    [SerializeField]
    private StorePanel storePanel;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (UIManager)FindObjectOfType(typeof(UIManager));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(UIManager) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        StoreManager.Instance.onStockAdded += AddToStorePanel;
    }

    void ShowStore()
    {
        
    }

    void AddToStorePanel(Guid guid, PlantData data)
    {
        storePanel.AddStoreItem(guid, data);
    }
}
