using System;
using Managers;
using Store;
using TMPro;
using UnityEngine;

namespace UIElements
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI moneyText;
        private void Awake()
        {
            moneyText = GetComponent<TextMeshProUGUI>();
            moneyText.SetText(StoreManager.Instance.AvailableCurrency.ToString());
            StoreManager.Instance.onItemBought += UpdateText;
        }

        private void UpdateText(Guid guid, StoreItem storeItem)
        {
            moneyText.text = StoreManager.Instance.AvailableCurrency.ToString();
        }
    }
}
