using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField] private LayerMask gridLayer; // Layer mask for grid objects
    private MeasureArrow measureArrow;
    private MeasureFiledUiManager measureFiledUiManager;

    private void OnMouseEnter()
    {
        measureArrow = GameObject.FindGameObjectWithTag("Manager").GetComponent<MeasureArrow>();
        measureFiledUiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MeasureFiledUiManager>();

        if (measureArrow.GetIsMeassureing())
        {
            Vector3 measureArrowStart = measureArrow.GetStart();
            Vector3 center = gameObject.GetComponent<Collider>().bounds.center;

            float deltaX = Mathf.Abs(measureArrowStart.x - center.x);
            float deltaY = Mathf.Abs(measureArrowStart.y - center.y);
            float deltaZ = Mathf.Abs(measureArrowStart.z - center.z);

            int squaresCount = Mathf.Max(Mathf.CeilToInt(deltaX), Mathf.CeilToInt(deltaY), Mathf.CeilToInt(deltaZ));

            squaresCount = Mathf.Max(0, squaresCount - 1);

            measureFiledUiManager.UpdateText(squaresCount*5);

            measureArrow.AdjustObjectBetweenTargets(center);  // Call the function from ExampleClass
        }
    }
}