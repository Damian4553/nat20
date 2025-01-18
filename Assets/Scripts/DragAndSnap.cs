using Unity.Netcode;
using UnityEngine;

public class DragAndSnap : NetworkBehaviour
{
    [SerializeField] private LayerMask snapLayer; // Layer mask for objects to snap to (set in inspector)
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private GameObject closestSnapObject;
    private bool snap = true;

    void Start()
    {
        gameObject.GetComponent<NetworkObject>().Spawn();

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
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, mainCamera.WorldToScreenPoint(position).z));

            // Update position, keeping the original Z-axis position
            position = new Vector3(mousePosition.x, mousePosition.y, position.z);
            transform.position = position;

            FindClosestSnapObject();
        }
    }

    void OnMouseDown()
    {
        // Prevent dragging if the mouse is over a UI element
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        // Start dragging the object if it belongs to the "Token" layer
        if (gameObject.layer == LayerMask.NameToLayer("Token"))
        {
            isDragging = true;
            // offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseUp()
    {

        if (isDragging && closestSnapObject != null && snap)
        {
            SnapToObject(closestSnapObject);
            SubmitPositionRequestServerRpc(transform.position); // Wysyłamy zmiany na serwer
        }

        isDragging = false;
    }

    private void FindClosestSnapObject()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        Bounds objectBounds = objectRenderer.bounds;

        Collider[] colliders =
            Physics.OverlapSphere(new Vector3(objectBounds.min.x, objectBounds.max.y, transform.position.z), 0.3f, snapLayer);
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

                Vector3 objectTopLeft = new Vector3(objectBounds.min.x, objectBounds.max.y, transform.position.z);
                Vector3 gridTopLeft = new Vector3(targetBounds.min.x, targetBounds.max.y, transform.position.z);

                Vector3 snappingOffset = gridTopLeft - objectTopLeft;

                transform.position += snappingOffset;
            }
            else
            {
                transform.position = target.transform.position;
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SubmitPositionRequestServerRpc(Vector3 newPosition)
    {

        transform.position = newPosition;
        UpdatePositionClientRpc(newPosition);
    }
    
    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition)
    {
        // Synchronizacja pozycji na wszystkich klientach
        transform.position = newPosition;
    }
}
