using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [SerializeField] private float move_speed = 0f;
    [SerializeField] private float time_human_control_seconds = 0f;
    [SerializeField] private float time_ai_control_seconds = 0f;

    /*************************************************/

    private Rigidbody2D ref_rbody = null;
    private SpriteRenderer ref_sprite_renderer = null;

    private float input_h_axis = 0f;
    private float input_v_axis = 0f;
    private float last_control_change_time = 0f;
    private bool ai_control = false;

    public float GetSpeed() { return move_speed;}
    public bool GetAIControl() { return ai_control; }

    private void Awake()
    {
        ref_rbody = this.GetComponent<Rigidbody2D>();
        ref_sprite_renderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        last_control_change_time = Time.time;
    }

    private void Update()
    {
        // Check if it's time to change control modes
        float elapsed_time = Time.time - last_control_change_time;
        if (elapsed_time > (ai_control ? time_ai_control_seconds : time_human_control_seconds))
        {
            // Swap to the other
            ai_control = !ai_control;
            last_control_change_time = Time.time;

            // Reset velocity
            ref_rbody.velocity = new Vector2(0f, 0f);

            // Visual differentiation
            if (ai_control)
            {
                ref_sprite_renderer.color = Color.black;
            }
            else
            {
                ref_sprite_renderer.color = Color.white;
            }
        }

        input_h_axis = Input.GetAxisRaw("Horizontal");
        input_v_axis = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (ai_control)
        {
            return;
        }

        float vel_x = input_h_axis * move_speed * Time.fixedDeltaTime;
        float vel_y = input_v_axis * move_speed * Time.fixedDeltaTime;

        ref_rbody.velocity = new Vector2(vel_x, vel_y);
    }
}
