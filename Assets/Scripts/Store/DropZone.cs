using System;
using Managers;
using Plants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Store
{
    public class DropZone : Interactable
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Plant")
            {
                StoreManager.Instance.Sell(collider.GetComponent<Plant>());
            }
        }

        public override void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
        {
            if (ref_held_obj != null)
            {
                if (original_tag == "Plant")
                {
                    StoreManager.Instance.Sell(ref_held_obj.GetComponent<Plant>());
                }
                if (original_tag == "WaterCan")
                {
                    UIManager.Instance.SendAMessage("I don't want to sell my watering can :(");
                } 
            }
        }

        public override void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
        {
            
        }

        public override void StopInteracting()
        {
        }
    }
    
}
