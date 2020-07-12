using System;
using Managers;
using Plants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Store
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        private void Awake()
        {
            StoreManager.Instance.onItemBought += DropItem;
        }


        private void DropItem(Guid guid, StoreItem storeItem)
        {
            switch (storeItem.itemType)
            {
                case StoreItem.ItemType.PLANT:
                {
                    Plant plant = Instantiate(PlantManager.Instance.plantPrefab,spawnPoint.position,Quaternion.identity);
                    plant.PlantData = storeItem.plantData;
                    Rigidbody2D rb = plant.GetComponent<Rigidbody2D>();
                    rb.AddForce(
                        Quaternion.Euler(0, 0, Random.Range(-30,30))*Vector2.left*15f,
                        ForceMode2D.Impulse);
                    rb.AddTorque(10f,ForceMode2D.Impulse);
                    break;
                }
                case StoreItem.ItemType.CONSUMABLE:
                {
                    break;
                }
            }
        }
    }
}
