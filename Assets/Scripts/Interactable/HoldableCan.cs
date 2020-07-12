using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class HoldableCan : Interactable
{
    private WateringCan _wateringCan;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _wateringCan = GetComponent<WateringCan>();
    }

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
            ref_game_obj.GetComponent<Rigidbody2D>().simulated = false;
            transform.rotation = Quaternion.identity;
            ref_held_obj = ref_game_obj;
            SoundManager.Instance.PlayItemGrabSound();
            if (_wateringCan.WaterAmount > 50f)
            {
                _wateringCan.RunParticles();
            }
        
        }
    }

    public override void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
    {
    }

    public override void StopInteracting()
    {
        _rigidbody2D.simulated = true;
        _wateringCan.StopParticles();
        SoundManager.Instance.PlayItemGrabSound();
    }
    
}
