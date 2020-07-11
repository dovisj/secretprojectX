using System;
using System.Collections.Generic;
using Managers;
using Plants;
using Store;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class StorePanel : MonoBehaviour
    {
        [SerializeField]
        private StoreButton buttonPrefab;
        [SerializeField]
        private GameObject buttonHolder;

        private Dictionary<Guid, GameObject> buttonList;
        void Awake()
        {
            buttonList = new Dictionary<Guid, GameObject>();
        }

        public void AddStoreItem(Guid guid,StoreItem data)
        {
            if (!buttonList.ContainsKey(guid))
            {
                StoreButton storeButton = Instantiate(buttonPrefab, buttonHolder.transform);
                storeButton.description = data.description;
                storeButton.nameText = data.itemName;
                storeButton.priceText = data.price.ToString();
                Button button = storeButton.GetComponent<Button>();
                button.onClick.AddListener(delegate { BuyAction(guid); });
                buttonList.Add(guid,button.gameObject);
            }
        }

        private void BuyAction(Guid guid)
        {
            UIManager.Instance.storeDescription.priceText.text = "";
            UIManager.Instance.storeDescription.descriptionText.text = "";
            UIManager.Instance.storeDescription.nameText.text = "";
            StoreManager.Instance.Buy(guid);
            RemoveStoreItem(guid);
        }
    
        public void RemoveStoreItem(Guid guid)
        {
            GameObject listButton;
            if (buttonList.TryGetValue(guid,out listButton))
            {
                buttonList.Remove(guid);
                Destroy(listButton);
            }
        }
    }
}
