using UnityEngine;

public class DragAndSnap : MonoBehaviour
{
    public LayerMask snapLayer; // Layer mask for objects to snap to (set in inspector)
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
            // Update the position of the object while dragging
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = mousePosition + offset;

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, snapLayer); // Radius of 10 units
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
            transform.position = target.transform.position; // Snap to the closest object
        }
    }
}