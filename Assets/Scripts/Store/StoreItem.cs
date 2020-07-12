using Plants;
using UnityEngine;

namespace Store
{
    public class StoreItem
    {
        public int buyPrice;
        public int sellPrice;
        public string itemName;
        public string description;
        public Sprite sprite;
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
            buyPrice = plantData.price;
            itemName = plantData.plantName;
            description = plantData.description;
            sprite = plantData.seedSprite;
        }
        
        public StoreItem()
        {
            this.plantData = plantData;
        }
    }
}
