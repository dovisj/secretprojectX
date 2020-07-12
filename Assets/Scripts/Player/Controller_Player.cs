using System.Collections;
using System.Collections.Generic;
using Managers;
using Plants;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ref_sprite_renderer = null;
    [SerializeField] private Behavior_Grab_Zone script_grab_zone = null;
    [SerializeField] private Transform ref_hold_location = null;
    [SerializeField] private Animator ref_player_animator = null;

    [SerializeField] private string[] tag_interactable;
    //[SerializeField] private string[] tag_holdables; // Changed holdables to be a type of interactables
    [SerializeField] private float move_speed = 0f;
    [SerializeField] private float time_human_control_seconds = 0f;
    [SerializeField] private float time_ai_control_seconds = 0f;

    /*************************************************/

    private Rigidbody2D ref_rbody = null;
    private GameObject ref_held_object = null;

    private string tag_held_original = ""; // This is to ensure the player doesn't treat the held object
    private LayerMask layer_held_original; // This makes sure selection helper doesn't collide with held objects
    private float input_h_axis = 0f;
    private float input_v_axis = 0f;
    private float last_control_change_time = 0f;
    private bool ai_control = false;

    public float GetSpeed() { return move_speed;}
    public bool GetAIControl() { return ai_control; }
    public ref GameObject GetRefHeld() {return ref ref_held_object;}
    public ref string GetHeldOGTag() { return ref tag_held_original; }
    public ref LayerMask GetHeldOGLayer() { return ref layer_held_original; }
    
    public ref Behavior_Grab_Zone GetRefGrabZone() {return ref script_grab_zone;}

    public void ProcessInteract()
    {
        // Check if there's an object 
        GameObject ref_obj = script_grab_zone.GetGrabObj();
        if (ref_obj != null)
        {
            // Check if it's interactable and act on it if it is
            Interactable(ref_obj);

            // Check if it's holdable and act on it if it is; an object can be both 
            //Holdable(ref_obj);
        }
        else // Place held object if holding one
        {
            if (ref_held_object != null)
            {
                if (script_grab_zone.Place(ref_held_object)) // Returns true if successfully placed
                {
                    DropItem();
                }

                /*
               RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 8f, Vector2.zero,10f,PlantManager.Instance.PlantPlotLayerMask);
               foreach (RaycastHit2D hit in hits)
               {
                   if (hit.collider != null)
                   {
                       PlantingPlot plot = hit.collider.GetComponent<PlantingPlot>();
                       if (plot.isTaken)
                       {
                           //Process Wattering, and things.
                       };
                       if (script_grab_zone.Place(ref_held_object, plot))
                       {
                           ref_held_object = null;
                           break;
                       }
                   }else if (script_grab_zone.Place(ref_held_object)) // Returns true if successfully placed
                   {
                       ref_held_object = null;
                       break;
                   }
               }
               */
            }
        }
    }

    public void DropItem()
    {
        ref_held_object.tag = tag_held_original;
        ref_held_object.layer = layer_held_original;
        ref_held_object.GetComponent<Interactable>().StopInteracting();
        ref_held_object = null;
    }

    private void Awake()
    {
        ref_rbody = this.GetComponent<Rigidbody2D>();
        //ref_sprite_renderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        last_control_change_time = Time.time;
    }

    private void Update()
    {
        // Check if it's time to change control modes
        float elapsed_time = Time.time - last_control_change_time;
        if (elapsed_time > (ai_control ? time_ai_control_seconds : time_human_control_seconds) && (ai_control ? time_human_control_seconds : time_ai_control_seconds) > 0)
        {
            // Swap to the other
            ai_control = !ai_control;
            last_control_change_time = Time.time;

            // Reset velocity
            ref_rbody.velocity = new Vector2(0f, 0f);

            // Visual differentiation, temporary TODO
            if (ai_control)
            {
                SoundManager.Instance.PlayUnderControlMusic();
                Debug.Log("AI taken control");
                script_grab_zone.gameObject.SetActive(false);
                ref_sprite_renderer.color = Color.red;
            }
            else
            {
                SoundManager.Instance.PlayChillMusic();
                Debug.Log("Player taken control");
                script_grab_zone.gameObject.SetActive(true);
                ref_sprite_renderer.color = Color.white;
            }
        }

        if (ai_control)
        {
            return;
        }

        // Movement input
        input_h_axis = Input.GetAxisRaw("Horizontal");
        input_v_axis = Input.GetAxisRaw("Vertical");

        // Grab input
        if (Input.GetButtonDown("Interact"))
        {
            ProcessInteract();
        }
    }

    private void FixedUpdate()
    {
        if (ref_held_object != null)
        {
            // Update location of grabbed object, TODO: Make this a joint that can break if moving too fast
            ref_held_object.transform.position = ref_hold_location.transform.position;
        }

        if (!ai_control)
        {
            float vel_x = input_h_axis * move_speed * Time.fixedDeltaTime;
            float vel_y = input_v_axis * move_speed * Time.fixedDeltaTime;

            ref_rbody.velocity = new Vector2(vel_x, vel_y);
        }

        // Handles for the AI part as well
        float rbody_vel_x = ref_rbody.velocity.x;
        float rbody_vel_y = ref_rbody.velocity.y;
        transform.localScale = new Vector2(Mathf.Sign(rbody_vel_x), transform.localScale.y);
        ref_player_animator.SetFloat("abs_vel_x", Mathf.Abs(rbody_vel_x));
        ref_player_animator.SetFloat("vel_y", rbody_vel_y);
    }

    private void Interactable(GameObject ref_obj)
    {
        bool can_interact = false;

        foreach (string str in tag_interactable)
        {
            if (ref_obj.tag == str)
            {
                can_interact = true;
            }
        }

        if (can_interact)
        {
            ref_obj.GetComponent<Interactable>().Interact(ref ref_held_object, ref tag_held_original, ref layer_held_original);
        }
    }

    private void Holdable(GameObject ref_obj)
    {
        /*
        bool can_hold = false;

        foreach (string str in tag_holdables)
        {
            if (ref_obj.tag == str)
            {
                can_hold = true;
            }
        }

        if (can_hold && ref_held_object == null) // Change here and add stack structure if more than one items can be held at once
        {
            tag_held_original = ref_obj.tag;
            layer_held_original = ref_obj.layer;
            ref_obj.tag = "Held";
            ref_obj.layer = LayerMask.NameToLayer("Held");
            ref_held_object = ref_obj;
            //ref_held_object.transform.parent = transform.parent; // What is this line for?
        }
        */
    }
}
