using UnityEngine;

public class MeasureFiledUiManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasPrefab;         // Reference to the TextMeshPro prefab
    private GameObject instantiatedCanvas; // Instantiated TextMeshPro object
    private bool isVisible = false;              // Visibility state
    private Camera mainCamera;                    // Reference to the main camera

    
    
    void Start()
    {
        // If no main camera is assigned, use the main camera in the scene
            mainCamera = Camera.main;
    }

    void Update()
    {
        // Check for mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            isVisible = true;

            if (instantiatedCanvas == null)
            {
                // Instantiate the prefab at the cursor position
                Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                cursorWorldPosition.z = 0;
                instantiatedCanvas = Instantiate(canvasPrefab, cursorWorldPosition, Quaternion.identity);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isVisible = false;
        }

        // Update visibility of the instantiated TextMeshPro object
        if (instantiatedCanvas != null)
        {
            instantiatedCanvas.SetActive(isVisible);

            if (isVisible)
            {
                // Update position
                instantiatedCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().rectTransform.position = Input.mousePosition + new Vector3(35,-15);
            }
        }

    }
    
    public void UpdateText(int newText)
    {
        if (instantiatedCanvas != null)
        {
            TMPro.TextMeshProUGUI textComponent = instantiatedCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textComponent.text = newText.ToString() + " ft";
        }
    }

    public void DestroyCanvas()
    {
        Destroy(instantiatedCanvas);
    }

}