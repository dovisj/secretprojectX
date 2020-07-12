using System;
using Plants;
using Store;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            StoreManager.Instance.onItemBought += CheckEndCondition;
            StoreManager.Instance.onItemSold += CheckWinCondition;
        }

        private void CheckEndCondition(Guid guid, StoreItem storeItem)
        {
            if (StoreManager.Instance.AvailableCurrency < -1000)
            {
                SceneManager.LoadScene (2);
            }
        }
        
        private void CheckWinCondition(Plant plantData)
        {
            if (StoreManager.Instance.AvailableCurrency > 10000)
            {
                SceneManager.LoadScene (0);
            }
        }
    }
}
