
using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using MEC;
using Plants;
using Store;
using TMPro;
using UIElements;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Queue<string> messageQueue;
    protected static UIManager instance;
    [SerializeField]
    public StorePanel storePanel;
    [SerializeField]
    private MessageCenter messageCenter;
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
        messageQueue = new Queue<string>();
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

    public void SendAMessage(string message)
    {
        Timing.RunCoroutine(SetMessage(message));
    }

    IEnumerator<float> SetMessage(string message)
    {
        messageCenter.SetMessage(message);
        yield return Timing.WaitForSeconds(3);
        messageCenter.SetMessage("");
    }

    public void ToggleTutorialMessages()
    {
        
    }
}
