using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 0.1f; // Speed for dragging the camera
    [SerializeField] private float zoomSpeed = 5f; // Speed for zooming in and out
    [SerializeField] private float minZoom = 2f; // Minimum zoom level (for orthographic or field of view)
    [SerializeField] private float maxZoom = 50f; // Maximum zoom level (for orthographic or field of view)
    [SerializeField] private GameObject container;
    [SerializeField] private GridManager gridManager;

    private Vector3 dragOrigin; // Tracks the initial position of the mouse during dragging
    private Bounds cameraBounds;

    private void Start()
    {
        if (container != null)
        {
            CalculateBounds();
        }
        if (gridManager != null)
        {
            gridManager.OnGridResized += CalculateBounds; // Event handler for grid resizing
        }
        
    }

    private void CalculateBounds()
    {
        // Calculate the bounds of the border object
        Renderer renderer = container.GetComponent<Renderer>();
        if (renderer != null)
        {
            cameraBounds = renderer.bounds;
        }
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampCameraPosition();
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
        }
    }
    
    private void ClampCameraPosition()
    {
        if (container == null) return;

        Camera cam = GetComponent<Camera>();

        if (cam.orthographic)
        {
            float camHeight = cam.orthographicSize * 2f;
            float camWidth = camHeight * cam.aspect;

            float moveRangeX = camWidth / 5f;
            float moveRangeY = camHeight / 5f;

            Vector3 newPosition = transform.position;

            newPosition.x = Mathf.Clamp(newPosition.x, cameraBounds.min.x + moveRangeX, cameraBounds.max.x - moveRangeX);
            newPosition.y = Mathf.Clamp(newPosition.y, cameraBounds.min.y + moveRangeY, cameraBounds.max.y - moveRangeY);

            transform.position = newPosition;
        }
    }
}