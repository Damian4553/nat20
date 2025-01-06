using System;
using UnityEngine;

public class ScaleDragAndSnap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // Assign your main camera in the Inspector
    [SerializeField] private float edgeThreshold = 0.1f; // Edge detection threshold (in local scale units)
    [SerializeField] private LayerMask snapLayer; // Layer mask for objects to snap to (set in inspector)
    [SerializeField] private LayerMask interactableLayer; // Assign this in the Inspector


    private bool isScaling = false;
    private bool isDragging = false;
    private Vector3 initialMousePosition;
    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Vector2 scaleDirection;
    private GameObject closestSnapObject;
    private bool snap = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            snap = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            snap = true;
        }

        if (isScaling)
        {
            PerformScaling();
        }
        else if (isDragging)
        {
            PerformDragging();
        }
        
    }
    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            scaleDirection = CalculateScaleDirection(mousePosition);

            if (scaleDirection != Vector2.zero)
            {
                StartScaling(mousePosition);
            }
            else
            {
                StartDragging();
            }
        }
    }


    private void OnMouseUp()
    {
        if (isScaling)
        {
            StopScaling();
        }
        else if (isDragging)
        {
            StopDragging();
        }
    }

    private void StartScaling(Vector3 mousePosition)
    {
        isScaling = true;
        initialMousePosition = mousePosition;
        initialScale = transform.localScale;
        initialPosition = transform.position;
    }

    private void PerformScaling()
    {
        Vector3 currentMousePosition = GetMouseWorldPosition();
        Vector3 delta = currentMousePosition - initialMousePosition;

        // Scale and position change based on the scaling direction
        Vector3 newScale = transform.localScale;
        Vector3 newPosition = transform.position;

        if (scaleDirection.x != 0)
        {
            float deltaX = delta.x * scaleDirection.x;
            newScale.x = Mathf.Max(0.1f, newScale.x + deltaX);
            newPosition.x += (deltaX / 2f) * scaleDirection.x;
        }

        if (scaleDirection.y != 0)
        {
            float deltaY = delta.y * scaleDirection.y;
            newScale.y = Mathf.Max(0.1f, newScale.y + deltaY);
            newPosition.y += (deltaY / 2f) * scaleDirection.y;
        }

        // Apply new scale and position
        transform.localScale = newScale;
        transform.position = newPosition;

        // Update initial values for continuous scaling
        initialMousePosition = currentMousePosition;
    }

    private void StopScaling()
    {
        isScaling = false;
    }

    private Vector2 CalculateScaleDirection(Vector3 mouseWorldPosition)
    {
        Vector3 objectCenter = transform.position; // Position of the object in world space
        Vector3 halfScale = transform.localScale / 2f; // Half the scale of the object
        Vector3 scaledEdgeThreshold = new Vector3(edgeThreshold, edgeThreshold, 0f); // Edge threshold in world space

        Vector2 direction = Vector2.zero;

        // Calculate distances from mouse to edges
        float distanceToRightEdge = objectCenter.x + halfScale.x - mouseWorldPosition.x;
        float distanceToLeftEdge = mouseWorldPosition.x - (objectCenter.x - halfScale.x);
        float distanceToTopEdge = objectCenter.y + halfScale.y - mouseWorldPosition.y;
        float distanceToBottomEdge = mouseWorldPosition.y - (objectCenter.y - halfScale.y);

        // Check horizontal edges
        if (distanceToRightEdge < scaledEdgeThreshold.x && distanceToRightEdge > 0)
            direction.x = 1; // Near right edge
        else if (distanceToLeftEdge < scaledEdgeThreshold.x && distanceToLeftEdge > 0)
            direction.x = -1; // Near left edge

        // Check vertical edges
        if (distanceToTopEdge < scaledEdgeThreshold.y && distanceToTopEdge > 0)
            direction.y = 1; // Near top edge
        else if (distanceToBottomEdge < scaledEdgeThreshold.y && distanceToBottomEdge > 0)
            direction.y = -1; // Near bottom edge

        return direction;
    }

    private void StartDragging()
    {
        isDragging = true;
    }

    private void PerformDragging()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

        // Find the closest object to snap to
        FindClosestSnapObject();
    }

    private void StopDragging()
    {
        if (closestSnapObject != null && snap)
        {
            SnapToObject(closestSnapObject);
        }
        isDragging = false;
    }

    private void FindClosestSnapObject()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        Bounds objectBounds = objectRenderer.bounds;

        Collider[] colliders = Physics.OverlapSphere(new Vector3(objectBounds.min.x, objectBounds.max.y, transform.position.z), 0.3f, snapLayer); // Radius of 0.3 units
        float closestDistance = Mathf.Infinity;

        closestSnapObject = null;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSnapObject = collider.gameObject;
            }
        }
    }

    private void SnapToObject(GameObject target)
    {
        if (target != null)
        {
            Renderer objectRenderer = GetComponent<Renderer>();
            Renderer targetRenderer = target.GetComponent<Renderer>();

            if (objectRenderer != null && targetRenderer != null)
            {
                Bounds objectBounds = objectRenderer.bounds;
                Bounds targetBounds = targetRenderer.bounds;

                // Calculate the top-left corner of the object and grid
                Vector3 objectTopLeft = new Vector3(objectBounds.min.x, objectBounds.max.y, transform.position.z);
                Vector3 gridTopLeft = new Vector3(targetBounds.min.x, targetBounds.max.y, transform.position.z);

                // Align object's top-left corner to grid's top-left corner
                Vector3 snappingOffset = gridTopLeft - objectTopLeft;

                // Apply the offset
                transform.position += snappingOffset;
            }
            else
            {
                // Fallback to snapping to the target's position
                transform.position = target.transform.position;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z); // Distance from camera
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
