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
            original_tag = gameObject.tag;
            original_layer = gameObject.layer;
            gameObject.tag = "Held";
            gameObject.layer = LayerMask.NameToLayer("Held");
            ref_held_obj = this.gameObject;
        }
    }
}
