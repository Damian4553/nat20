using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
     public float dragSpeed = 0.1f;         // Speed for dragging the camera
    public float zoomSpeed = 5f;          // Speed for zooming in and out
    public float minZoom = 2f;            // Minimum zoom level (for orthographic or field of view)
    public float maxZoom = 50f;           // Maximum zoom level (for orthographic or field of view)

    private Vector3 dragOrigin;           // Tracks the initial position of the mouse during dragging

    void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    private void HandleDrag()
    {
        // Check if the middle mouse button is pressed
        if (Input.GetMouseButtonDown(2))
        {
            // Record the initial position of the mouse
            dragOrigin = Input.mousePosition;
            return;
        }

        // If the middle mouse button is held down
        if (Input.GetMouseButton(2))
        {
            // Calculate the difference in mouse position
            Vector3 difference = Input.mousePosition - dragOrigin;

            // Move the camera in the opposite direction of the drag
            Vector3 move = new Vector3(-difference.x * dragSpeed, -difference.y * dragSpeed, 0);
            transform.Translate(move, Space.World);

            // Update drag origin for the next frame
            dragOrigin = Input.mousePosition;
        }
    }

    private void HandleZoom()
    {
        // Get the scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Camera cam = GetComponent<Camera>();

            if (cam.orthographic)
            {
                // Adjust the orthographic size for 2D cameras
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                // Adjust the field of view for 3D cameras
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);
            }
        }
    }
}