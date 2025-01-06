using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaletest : MonoBehaviour
{
    
    public GameObject objectToScale;
    public GameObject targetA;
    public GameObject targetB;

    void Start()
    {
        AdjustObjectBetweenTargets(objectToScale, targetA, targetB);
    }
    
    public static void AdjustObjectBetweenTargets(GameObject objectToAdjust, GameObject targetA, GameObject targetB)
    {
        if (objectToAdjust == null || targetA == null || targetB == null)
        {
            Debug.LogError("One or more objects are null.");
            return;
        }

        // Get the positions of the target objects
        Vector3 positionA = targetA.transform.position;
        Vector3 positionB = targetB.transform.position;

        // Calculate the midpoint between the two targets
        Vector3 midpoint = (positionA + positionB) / 2;

        // Calculate the distance between the two targets
        float distance = Vector3.Distance(positionA, positionB);

        // Set the position of the object to the midpoint
        objectToAdjust.transform.position = midpoint;

        // Preserve the X and Y rotation, and calculate the new Z rotation
        Vector3 direction = positionB - positionA;
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate angle in degrees
        Vector3 eulerRotation = objectToAdjust.transform.eulerAngles;
        eulerRotation.z = zRotation; // Update only the Z rotation
        objectToAdjust.transform.rotation = Quaternion.Euler(eulerRotation);

        // Adjust the scale of the object
        Vector3 newScale = objectToAdjust.transform.localScale;
        newScale.x = distance; // Assuming scaling happens along the X-axis
        objectToAdjust.transform.localScale = newScale;
    }
}
