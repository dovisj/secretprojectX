using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class AIStore : Interactable
{

    public override void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
        
    }

    public override void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
        if (StoreManager.Instance.currentStock.Count > 0)
        {
            StoreManager.Instance.BuyRandom();
        }
    }

    public override void StopInteracting()
    {

    }
}
