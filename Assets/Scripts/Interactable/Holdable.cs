using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : Interactable
{
    public override void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
        // If the entity interacting with this plant is not holding anything, make it hold this plant
        if (ref_held_obj == null) // Not already holding an object
        {
            GameObject ref_game_obj = this.gameObject;
            original_tag = ref_game_obj.tag;
            original_layer = ref_game_obj.layer;
            ref_game_obj.tag = "Held";
            ref_game_obj.layer = LayerMask.NameToLayer("Held");
            ref_held_obj = ref_game_obj;
            if (original_tag == "Plant")
            {
                SoundManager.Instance.PlayRandomFlowerGetSound();
            }
        }
    }

    public override void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
    }

    public override void StopInteracting()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
