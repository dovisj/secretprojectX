using Plants;
using UnityEngine;

namespace Store
{
    public class StoreItem
    {
        public int price;
        public string itemName;
        public string description;
        public enum ItemType
        {
            PLANT,
            CONSUMABLE
        }

        public ItemType itemType;

        public PlantData plantData;
        //public Consumable consumable;
        public StoreItem(PlantData plantData)
        {
            itemType = ItemType.PLANT;
            this.plantData = plantData;
            price = plantData.price;
            itemName = plantData.name;
            description = plantData.description;
        }
        
        public StoreItem()
        {
            this.plantData = plantData;
        }
    }
}
