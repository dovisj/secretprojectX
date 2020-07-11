using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Grab_Zone : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ref_parent_rbody = null;

    [SerializeField] private float offset_distance = 0f;
    [SerializeField] private float start_move_angle_deg = 0f;

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

    private void Start()
    {
        starting_loc = transform.localPosition;
        move_angle = Mathf.Deg2Rad * start_move_angle_deg;
    }

    private void Update()
    {
        // Get move angle
        if (ref_parent_rbody.velocity.magnitude > 0)
        {
            Vector2 vel = ref_parent_rbody.velocity;

            move_angle = Mathf.Atan2(vel.y, vel.x); // Radians
        }

        // Move local position of grab zone by offset and move angle
        transform.localPosition = starting_loc + (new Vector2(Mathf.Cos(move_angle), Mathf.Sin(move_angle)) * offset_distance);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Holdable")
        {
            obj_in_zone = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        obj_in_zone = null;
    }
}
