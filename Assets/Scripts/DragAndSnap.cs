using UnityEngine;

public class DragAndSnap : MonoBehaviour
{
    [SerializeField] private LayerMask snapLayer; // Layer mask for objects to snap to (set in inspector)
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private GameObject closestSnapObject;
    private bool snap = true;
    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
    }

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

        if (isDragging)
        {
            // Get the mouse position in world space
            var position = transform.position;
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(position).z));
        
            // Update position, keeping the original Z-axis position
            position = new Vector3(mousePosition.x, mousePosition.y, position.z);
            transform.position = position;

            // Find the closest object to snap to
            FindClosestSnapObject();
        }
    }

    void OnMouseDown()
    {
        // Start dragging the object
        if (gameObject.layer == LayerMask.NameToLayer("Token"))
        {
            isDragging = true;
            // offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseUp()
    {
        // Stop dragging the object and snap it
        if (isDragging && closestSnapObject != null && snap)
        {
            // Snap to the closest object
            SnapToObject(closestSnapObject);
        }

        isDragging = false;
    }

    private void FindClosestSnapObject()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        Bounds objectBounds = objectRenderer.bounds;

        Collider[] colliders = Physics.OverlapSphere(new Vector3(objectBounds.min.x, objectBounds.max.y, transform.position.z), .3f, snapLayer); // Radius of 10 units
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

    
}
