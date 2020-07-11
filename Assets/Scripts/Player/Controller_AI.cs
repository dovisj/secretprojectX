using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_AI : MonoBehaviour
{
    [SerializeField] private Controller_Player script_controller_player = null;

    [SerializeField] private float ai_speed_multiplier = 1f;

    /*************************************************/

    private GameObject current_target = null;
    
    private float speed = 0;
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
        if (current_target == null)
        {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!ai_control || current_target == null)
        {
            return;
        }

        if (collision.collider.gameObject.GetInstanceID() == current_target.GetInstanceID())
        {
            // Do interaction

            // Reset target
            current_target = null;
        }
        else if (collision.collider.tag == "Interactable")
        {
            
        }
    }
}
