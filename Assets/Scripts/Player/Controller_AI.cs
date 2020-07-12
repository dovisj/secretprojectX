using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_AI : MonoBehaviour
{
    [SerializeField] private Controller_Player script_controller_player = null;

    [SerializeField] private string[] tag_seekables;
    [SerializeField] private float ai_speed_multiplier = 1f;
    
    // How long to wait after interacting with a target before finding new target
    // (Essentially not doing anything; thus timeout can override this so this value should be smaller than the timeout)
    [SerializeField] private float seeking_delay_seconds = 0f;
    [SerializeField] private float seeking_timeout_seconds = 60f; // How long to wait before selecting a new target (ignores the seeking_delay). Useful when AI gets stuck.

    /*************************************************/

    private GameObject current_target = null;
    
    private float speed = 0;
    private float last_seek_time = 0;
    private bool ai_control = false;

    private void Start()
    {
        speed = script_controller_player.GetSpeed() * ai_speed_multiplier;
    }

    private void Update()
    {
        ai_control = script_controller_player.GetAIControl();

        if (!ai_control)
        {
            current_target = null;
            return;
        }

        // Find new interaction target 
        float elapsed_time = Time.time - last_seek_time;
        if ((current_target == null && elapsed_time > seeking_delay_seconds) || elapsed_time > seeking_timeout_seconds)
        {
            GameObject[][] seek_objs = new GameObject[tag_seekables.Length][];
            int total_seek_objs = 0;

            // Might be super slow to run, TODO: Optimize
            for (int i = 0; i < tag_seekables.Length; ++i)
            {
                seek_objs[i] = GameObject.FindGameObjectsWithTag(tag_seekables[i]);
                total_seek_objs += seek_objs[i].Length;
            }

            int seek_index = (int)((float)total_seek_objs * Random.value);

            int cumulative_length = 0;
            bool found = false;
            for (int i = 0; i < seek_objs.Length && !found; ++i)
            {
                int group_size = seek_objs[i].Length;
                if (seek_index >= cumulative_length && seek_index < (cumulative_length + group_size))
                {
                    current_target = seek_objs[i][seek_index - cumulative_length];
                    found = true;
                }
                else
                {
                    cumulative_length += group_size;
                }
            }

            /*
            // Get all possible objects
            GameObject[] seek_objects_interactable = GameObject.FindGameObjectsWithTag("Interactable");
            GameObject[] seek_objects_holdable = GameObject.FindGameObjectsWithTag("Holdable");

            int total_length = seek_objects_interactable.Length + seek_objects_holdable.Length;

            // Randomly choose an object to interact with
            int seek_index = (int)((float) total_length * Random.value);
            if (seek_index < seek_objects_interactable.Length)
            {
                current_target = seek_objects_interactable[seek_index];
            }
            else if (seek_index < (seek_objects_interactable.Length + seek_objects_holdable.Length))
            {
                current_target = seek_objects_holdable[seek_index - seek_objects_interactable.Length];
            } // List can be expanded like this with more tags
            */

            last_seek_time = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (!ai_control || current_target == null)
        {
            return;
        }

        Vector2 seek_pos = current_target.transform.position;

        Debug.DrawLine(transform.position, seek_pos);
        transform.position = Vector2.MoveTowards(transform.position, seek_pos, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerStay2D(Collider2D collider) // Use TriggerStay because same target could be chosen again (static Plots, ect)
    {
        if (!ai_control || current_target == null)
        {
            return;
        }

        CheckTarget(collider);
    }

    private void OnCollisionStay2D(Collision2D collision) // Use CollisionStay because same target could be chosen again (static Plots, ect)
    {
        if (!ai_control || current_target == null)
        {
            return;
        }

        CheckTarget(collision.collider);
    }

    private void CheckTarget(Collider2D collider)
    {
        if (collider.gameObject.GetInstanceID() == current_target.GetInstanceID())
        {
            // Do interaction
            collider.gameObject.GetComponent<Interactable>().Interact(ref script_controller_player.GetRefHeld(),
                    ref script_controller_player.GetHeldOGTag(), ref script_controller_player.GetHeldOGLayer());

            // Reset target
            current_target = null;
            last_seek_time = Time.time;
        }
    }
}
