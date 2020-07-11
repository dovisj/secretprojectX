using System;
using System.Collections.Generic;
using Plants;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class StorePanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private GameObject buttonHolder;

        private Dictionary<Guid, GameObject> buttonList;
        void Awake()
        {
            buttonList = new Dictionary<Guid, GameObject>();
        }

        public void AddStoreItem(Guid guid,PlantData data)
        {
            if (!buttonList.ContainsKey(guid))
            {
                GameObject buttonObject = Instantiate(buttonPrefab, buttonHolder.transform);
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(delegate { BuyAction(guid); });
                buttonList.Add(guid,button.gameObject);
            }
        }

        private void BuyAction(Guid guid)
        {
            StoreManager.Instance.Buy(guid);
            RemoveStoreItem(guid);
        }
    
        private void RemoveStoreItem(Guid guid)
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
