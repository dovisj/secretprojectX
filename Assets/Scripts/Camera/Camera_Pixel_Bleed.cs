using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Pixel_Bleed : MonoBehaviour
{
    // Pixel bleeding occurs when camera resolution is non-integer

    [SerializeField] private Camera ref_camera = null;

    [SerializeField] private float snap_x = 1f;
    [SerializeField] private float snap_y = 1f;

    private void Update()
    {
        Vector2 new_size = new Vector2(Snap(Screen.width, snap_x), Snap(Screen.height, snap_y));
        ref_camera.pixelRect = new Rect(ref_camera.pixelRect.position, new_size);
    }

    private float Snap(float x, float snap_size)
    {
        return Mathf.Floor(x / snap_size) * snap_size;
    }
}
