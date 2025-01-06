using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private GameObject tokenLayer;
    [SerializeField] private GameObject gmLayer;
    [SerializeField] private GameObject mapLayer;

    [SerializeField] private Button tokenButton;
    [SerializeField] private Button gmButton;
    [SerializeField] private Button mapButton;

    [Range(0f, 1f)] [SerializeField] private float transparency = 0.5f;

    private void Start()
    {
        // Assign button click events
        tokenButton.onClick.AddListener(OnTokenButtonClick);
        gmButton.onClick.AddListener(OnGMButtonClick);
        mapButton.onClick.AddListener(OnMapButtonClick);
        UpdateLayerState(mapLayer, false, false);
        UpdateLayerState(gmLayer, false, false, transparency);
        UpdateLayerState(tokenLayer, true, true, 1f);

    }

    private void OnTokenButtonClick()
    {
        UpdateLayerState(mapLayer, false, false);
        UpdateLayerState(gmLayer, false, false, transparency);
        UpdateLayerState(tokenLayer, true, true, 1f);
    }

    private void OnGMButtonClick()
    {
        UpdateLayerState(mapLayer, false, false);
        UpdateLayerState(tokenLayer, false, false, transparency);
        UpdateLayerState(gmLayer, true, true, 1f);
    }

    private void OnMapButtonClick()
    {
        UpdateLayerState(tokenLayer, false, false, transparency);
        UpdateLayerState(gmLayer, false, false, transparency);
        UpdateLayerState(mapLayer, true, true, 1f);
    }

    private void UpdateLayerState(GameObject layer, bool enableDragSnap, bool enableScaleDragSnap, float? alpha = null)
    {
        if (layer == null) return;

        foreach (Transform child in layer.transform)
        {
            // Update DragAndSnap script
            var dragAndSnap = child.GetComponent<DragAndSnap>();
            if (dragAndSnap != null)
            {
                dragAndSnap.enabled = enableDragSnap;
            }

            // Update ScaleDragAndSnap script
            var scaleDragAndSnap = child.GetComponent<ScaleDragAndSnap>();
            if (scaleDragAndSnap != null)
            {
                scaleDragAndSnap.enabled = enableScaleDragSnap;
            }

            // Update transparency
            if (alpha.HasValue)
            {
                SetTransparency(child, alpha.Value);
            }
        }
    }

    private void SetTransparency(Transform child, float alpha)
    {
        Renderer renderer = child.GetComponent<Renderer>();
        if (renderer != null)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    Color color = material.color;
                    color.a = alpha;
                    material.color = color;

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
