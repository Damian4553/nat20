using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private Vector3 originalPosition;

    [SerializeField] private string snapLayer = "Token"; // Layer to determine snappable objects
    [SerializeField] private float tileSize = 1f; // Size of each grid tile (assume square tiles)

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleDragAndDrop();
    }

    void HandleDragAndDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Check if the object is in the correct layer
                if (hitObject.layer == LayerMask.NameToLayer(snapLayer))
                {
                    selectedObject = hitObject;
                    originalPosition = selectedObject.transform.position;

                    // Calculate offset to maintain relative position during dragging
                    offset = selectedObject.transform.position - GetMouseWorldPosition();
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            // Update the position while dragging
            selectedObject.transform.position = GetMouseWorldPosition() + offset;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            SnapToGrid(selectedObject);
            selectedObject = null;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z); // Ensure depth is correct
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    void SnapToGrid(GameObject obj)
    {
        Vector3 objPosition = obj.transform.position;

        // Calculate the top-left corner based on the object's size and grid tile size
        Vector3 size = GetObjectSize(obj);
        Vector3 topLeftCorner = new Vector3(
            Mathf.Floor(objPosition.x / tileSize) * tileSize,
            Mathf.Ceil(objPosition.y / tileSize) * tileSize,
            objPosition.z // Maintain Z position
        );

        // Adjust position based on the size of the object
        Vector3 adjustedPosition = topLeftCorner + new Vector3(size.x / 2 * tileSize, -size.y / 2 * tileSize, 0);
        obj.transform.position = adjustedPosition;
    }

    Vector3 GetObjectSize(GameObject obj)
    {
        // Calculate the object's size in grid units
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 size = renderer.bounds.size / tileSize;
            return new Vector3(Mathf.Round(size.x), Mathf.Round(size.y), Mathf.Round(size.z));
        }
        return Vector3.one; // Default to 1x1 if no renderer is found
    }
}
