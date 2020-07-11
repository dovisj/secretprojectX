using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIElements
{
    public class StoreButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public string priceText;
        [SerializeField] public string nameText;
        [SerializeField] public string description;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        //Detect if the Cursor starts to pass over the GameObject
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            UIManager.Instance.storeDescription.priceText.text = priceText;
            UIManager.Instance.storeDescription.descriptionText.text = description;
            UIManager.Instance.storeDescription.nameText.text = nameText;
        }

        //Detect when Cursor leaves the GameObject
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            UIManager.Instance.storeDescription.priceText.text = "";
            UIManager.Instance.storeDescription.descriptionText.text = "";
            UIManager.Instance.storeDescription.nameText.text = "";
        }
    }
}