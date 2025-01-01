using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    // The parent object whose children will be made transparent
    [SerializeField] private GameObject parentObjectDisable;
    [SerializeField] private GameObject parentObjectEneble;

    // The transparency level (0 = fully transparent, 1 = fully opaque)
    [Range(0f, 1f)] [SerializeField] private float transparency = 0.5f;

    void Start()
    {
        // Check if the parent object is assigned
        if (parentObjectDisable != null)
        {
            // Get all child objects and set their transparency
            SetChildrenTransparency(parentObjectDisable.transform, transparency); //off
            TurnOffOnDragAndSnap(parentObjectDisable.transform, true);

            SetChildrenTransparency(parentObjectEneble.transform, 1f); //on
            TurnOffOnDragAndSnap(parentObjectEneble.transform, false);
        }
        else
        {
            Debug.LogWarning("Parent object is not assigned!");
        }
    }

    public void SetChildrenTransparency(Transform parent, float alpha)
    {
        // Iterate through all child objects
        foreach (Transform child in parent)
        {
            // Get the Renderer component of the child
            Renderer renderer = child.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Iterate through all materials of the renderer
                foreach (Material material in renderer.materials)
                {
                    // Ensure the material supports transparency
                    if (material.HasProperty("_Color"))
                    {
                        // Get the current color and set the alpha value
                        Color color = material.color;
                        color.a = alpha;
                        material.color = color;

                        // Enable transparency on the material's shader if necessary
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = 3000;
                    }
                }
            }
        }
    }

    void TurnOffOnDragAndSnap(Transform parent, bool turnOff = true)
    {
        // Iterate through all child objects
        foreach (Transform child in parent)
        {
            // Get the DragAndSnap script component of the child
            var dragAndSnap = child.GetComponent<DragAndSnap>();

            if (dragAndSnap != null && turnOff)
            {
                // Disable the DragAndSnap script
                dragAndSnap.enabled = false;
            }

            if (dragAndSnap != null && !turnOff)
            {
                // Enable the DragAndSnap script
                dragAndSnap.enabled = true;
            }


            // Recursive call to handle nested children
            if (child.childCount > 0)
            {
                TurnOffOnDragAndSnap(child, turnOff);
            }
        }
    }
}