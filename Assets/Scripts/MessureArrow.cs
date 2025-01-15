using UnityEngine;


public class MeasureArrow : MonoBehaviour
{
    public GameObject arrowPrefab; // The prefab for the arrow
    public LayerMask gridLayer; // Layer mask for grid objects

    private GameObject currentArrow; // Reference to the currently spawned arrow

    private Vector3 start; //stating position os messuring
    private Camera mainCamera;
    private bool isMeassureing = false;
    


    private void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
    }

    public Vector3 GetStart()
    {
        return start;
    }

    public bool GetIsMeassureing()
    {
        return isMeassureing;
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            isMeassureing = true;
            SpawnArrow();

            Vector3 mousePosition = Input.mousePosition;

            // Generate a ray from the camera through the screen point
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Perform a raycast to find the closest object hit by the ray
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                // Get the closest object's center
                Collider closestCollider = hitInfo.collider;
                if (closestCollider != null)
                {
                    start = GetObjectCenter(closestCollider);
                }
            }

        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isMeassureing = false;

            DestroyArrow();
        }
        
    }

    void SpawnArrow()
    {
        // Create the arrow at an arbitrary position (we'll move it immediately)
        currentArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);

        //set correct position of arrow
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // Get the center of the object
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                Vector3 gridCenter = renderer.bounds.center;

                // Adjust the position of the arrow so the left side is at the grid's center
                Vector3 arrowSize = currentArrow.GetComponent<Renderer>().bounds.size;
                currentArrow.transform.position = gridCenter - new Vector3(arrowSize.x * -0.5f, 0, 0);
            }
        }
    }

    public void DestroyArrow()
    {
        // Destroy the current arrow if it exists
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    public void AdjustObjectBetweenTargets(Vector3 end)
    {
        if (currentArrow == null)
        {
            return;
        }

        // Calculate the distance and direction between the start and end points
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        // Scale the arrow while keeping the left side (start) anchored
        Vector3 newScale = currentArrow.transform.localScale;
        newScale.x = Mathf.Max(0.1f, distance/2.75f); // Ensure minimal scale
        currentArrow.transform.localScale = newScale;

        // Adjust the position so the left side stays anchored at 'start' and shifted slightly to the right
        Vector3 newPosition = start + direction * (newScale.x * 1.35f ); // Compensate for scaling by moving the center of the arrow
        currentArrow.transform.position = newPosition;

        // Ensure the rotation is updated such that the arrow faces in the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    

    Collider GetClosestObject(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f); // Search within a small radius

        if (colliders.Length == 0)
            return null;

        Collider closest = colliders[0];
        float closestDistance = Vector3.Distance(position, closest.transform.position);

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(position, collider.transform.position);
            if (distance < closestDistance)
            {
                closest = collider;
                closestDistance = distance;
            }
        }

        return closest;
    }

    Vector3 GetObjectCenter(Collider collider)
    {
        Bounds bounds = collider.bounds;
        return bounds.center;
    }
}