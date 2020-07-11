
using System;
using System.Collections.Generic;
using Managers;
using Plants;
using Store;
using TMPro;
using UIElements;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    protected static UIManager instance;
    [SerializeField]
    private StorePanel storePanel;
    [SerializeField]
    private TextMeshProUGUI storeTimer;
    [SerializeField]
    public StoreDescription storeDescription;
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
        StoreManager.Instance.onStockRemoved += RemoveFromStorePanel;
    }

    void ShowDescription()
    {
        
    }

    void AddToStorePanel(Guid guid, StoreItem data)
    {
        storePanel.AddStoreItem(guid, data);
    }
    
    void RemoveFromStorePanel(Guid guid)
    {
        storePanel.RemoveStoreItem(guid);
    }

    public void SetStoreTimer(string time)
    {
        storeTimer.text = time;
    }
}
