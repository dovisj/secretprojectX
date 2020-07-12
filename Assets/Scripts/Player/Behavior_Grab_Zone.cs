using System.Collections;
using System.Collections.Generic;
using Plants;
using UnityEngine;

public class Behavior_Grab_Zone : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ref_parent_rbody = null;

    [SerializeField] private float offset_distance = 0f;
    [SerializeField] private float start_move_angle_deg = 0f;
    [SerializeField] private float snap_size = 1f;
    [SerializeField] private float snap_offset = 0f;

    private Vector2 starting_loc;
    private GameObject obj_in_zone = null;

    private float move_angle = 0f;

    public GameObject GetGrabObj() {return obj_in_zone;}
    
    public bool Place(GameObject ref_held_object)
    {
        if (obj_in_zone == null) // Nothing in way, can place
        {
            ref_held_object.transform.position = transform.position;
            return true;
        }
        return false;
    }
    
    /* Plot now takes care of item placed in it
    public bool Place(GameObject ref_held_object, PlantingPlot plot)
    {
        if (obj_in_zone == null) // Nothing in way, can place
        {
            ref_held_object.transform.position = plot.transform.position;
            ref_held_object.transform.parent = plot.transform;
            plot.PlaceOn(ref_held_object);
            return true;
        }
        return false;
    }
    */

    private void Start()
    {
        starting_loc = transform.localPosition;
        move_angle = Mathf.Deg2Rad * start_move_angle_deg;
        Move();
    }

    private void Update()
    {
        // Get move angle
        if (ref_parent_rbody.velocity.magnitude > 0)
        {
            Vector2 vel = ref_parent_rbody.velocity;

            move_angle = Mathf.Atan2(vel.y, vel.x); // Radians
        }

        Move();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        obj_in_zone = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        obj_in_zone = null;
    }

    private float Snap(float x)
    {
        return Mathf.Floor(x / snap_size) * snap_size + snap_offset;
    }

    private void Move()
    {
        // Move local position of grab zone by offset and move angle
        // Parent flips localScale.x to flip sprite, so multiply by it to correct positioning
        transform.localPosition = starting_loc + (new Vector2(Mathf.Cos(move_angle) * ref_parent_rbody.transform.localScale.x, Mathf.Sin(move_angle)) * offset_distance);
        // Snap to grid
        transform.position = new Vector2(Snap(transform.position.x), Snap(transform.position.y));
    }
}
