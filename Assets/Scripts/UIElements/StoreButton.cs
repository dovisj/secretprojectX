using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class StoreButton : MonoBehaviour
    {
        [SerializeField] private TextMeshPro priceText;
        [SerializeField] private TextMeshPro nameText;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }
    }
}
