using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class WaterWell : Interactable
{
    public override void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
        if (ref_held_obj != null) // Not holding anything
        {
            if (original_tag == "WaterCan")
            {
               WateringCan wateringCan = ref_held_obj.GetComponent<WateringCan>();
               wateringCan.WaterAmount = 100f;
               wateringCan.RunParticles();
                SoundManager.Instance.PlayWaterCanFill();
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
