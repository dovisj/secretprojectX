using System;
using Managers;
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
            EventManager.StartListening("ITEM_BOUGHT", UpdateText);
            EventManager.StartListening("ITEM_SOLD", UpdateText);
        }

        private void UpdateText()
        {
            moneyText.text = StoreManager.Instance.AvailableCurrency.ToString();
        }
    }
}
