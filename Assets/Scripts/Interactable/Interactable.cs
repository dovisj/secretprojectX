using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Held objects have their tags and layers modified
    public abstract void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer);
    
    public abstract void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer);
    public abstract void StopInteracting();
}
