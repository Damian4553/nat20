using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Button addButton;
    [SerializeField] private Button destroyButton;
    [SerializeField] private Button resizeButton;
    
    [SerializeField] private TMP_InputField inputFieldX;
    [SerializeField] private TMP_InputField inputFieldY;
    
    [SerializeField] private TokenManager tokenManager;


    private int lastActiveLayer; // 1 = token, 2 = gm, 3 = map

    [Range(0f, 1f)] [SerializeField] private float transparency = 0.5f;
    
    public event Action<float, float> OnResize;

    private void Start()
    {
        // Assign button click events
        tokenButton.onClick.AddListener(OnTokenButtonClick);
        gmButton.onClick.AddListener(OnGMButtonClick);
        mapButton.onClick.AddListener(OnMapButtonClick);
        addButton.onClick.AddListener(OnAddButtonClick);
        destroyButton.onClick.AddListener(OnDestroyButtonClick);
        resizeButton.onClick.AddListener(HandleResize);
        
        tokenManager.OnLayerChanged += HandleLayerChange;


        OnTokenButtonClick();
    }
    
    private void HandleLayerChange()
    {
        switch (lastActiveLayer)
        {
            case 1:
                OnTokenButtonClick();
                break;
            case 2:
                OnGMButtonClick();
                break;
            case 3:
                OnMapButtonClick();
                break;
        }
    }

    private void HandleResize()
    {
        // Parse input values
        if (float.TryParse(inputFieldX.text, out float x) && float.TryParse(inputFieldY.text, out float y))
        {
            OnResize?.Invoke(x, y); // Trigger resize event with the parsed values
        }
        else
        {
            Debug.LogError("Invalid input. Please enter valid numbers in the input fields.");
        }
    }


    private void OnAddButtonClick()
    {
        if (gameObject.GetComponent<CreateToken>().enabled)
        {
            gameObject.GetComponent<CreateToken>().enabled = false;
            switch (lastActiveLayer)
            {
                case 1:
                    OnTokenButtonClick();
                    break;
                case 2:
                    OnGMButtonClick();
                    break;
                case 3:
                    OnMapButtonClick();
                    break;
            }
        }
        else
        {
            gameObject.GetComponent<CreateToken>().enabled = true;
            gameObject.GetComponent<DestroyToken>().enabled = false;
            UpdateLayerState(mapLayer, false, 1f);
            UpdateLayerState(gmLayer, false, transparency);
            UpdateLayerState(tokenLayer, false, 1f);
        }
    }
    
    private void OnDestroyButtonClick()
    {
        
        if (gameObject.GetComponent<DestroyToken>().enabled)
        {
            gameObject.GetComponent<DestroyToken>().enabled = false;
            switch (lastActiveLayer)
            {
                case 1:
                    OnTokenButtonClick();
                    break;
                case 2:
                    OnGMButtonClick();
                    break;
                case 3:
                    OnMapButtonClick();
                    break;
            }
        }
        else
        {
            gameObject.GetComponent<DestroyToken>().enabled = true;
            gameObject.GetComponent<CreateToken>().enabled = false;
            UpdateLayerState(mapLayer, false, 1f);
            UpdateLayerState(gmLayer, true, transparency);
            UpdateLayerState(tokenLayer, true, 1f);
        }
    }

    private void OnTokenButtonClick()
    {
        UpdateLayerState(mapLayer, false, 1f);
        UpdateLayerState(gmLayer, false, transparency);
        UpdateLayerState(tokenLayer, true, 1f);
        lastActiveLayer = 1;
    }

    private void OnGMButtonClick()
    {
        UpdateLayerState(mapLayer, false, 1f);
        UpdateLayerState(tokenLayer, false, transparency);
        UpdateLayerState(gmLayer, true, 1f);
        lastActiveLayer = 2;
    }

    private void OnMapButtonClick()
    {
        UpdateLayerState(tokenLayer, false, transparency);
        UpdateLayerState(gmLayer, false, transparency);
        UpdateLayerState(mapLayer, true, 1f);
        lastActiveLayer = 3;
    }

    private void UpdateLayerState(GameObject layer, bool enableColliders, float alpha)
    {
        if (layer == null) return;

        foreach (Transform child in layer.transform)
        {
            // Update Collider state
            var boxCollider = child.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = enableColliders;
            }

            var circleCollider = child.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = enableColliders;
            }

            // Update transparency
            SetTransparency(child, alpha);
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
