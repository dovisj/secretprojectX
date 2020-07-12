using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Held objects have their tags and layers modified
    public abstract void Interact(ref GameObject ref_held_obj, string original_tag, LayerMask original_layer);
}
