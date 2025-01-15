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

            // Obliczanie różnicy między współrzędnymi
            float deltaX = Mathf.Abs(measureArrowStart.x - center.x);
            float deltaY = Mathf.Abs(measureArrowStart.y - center.y);  // Uwzględniamy także oś Y
            float deltaZ = Mathf.Abs(measureArrowStart.z - center.z);  // Uwzględniamy także oś Z

            // Liczymy liczbę kroków wzdłuż osi X, Y i Z
            int squaresCount = Mathf.Max(Mathf.CeilToInt(deltaX), Mathf.CeilToInt(deltaY), Mathf.CeilToInt(deltaZ));

            // Zmniejszamy wynik o 1, ponieważ punkt początkowy nie jest liczbą kroków
            squaresCount = Mathf.Max(0, squaresCount - 1);

            // Wyświetlamy wynik
            measureFiledUiManager.UpdateText(squaresCount*5);

            // Możesz również zaaktualizować pozycję strzałki
            measureArrow.AdjustObjectBetweenTargets(center);  // Call the function from ExampleClass
        }
    }
}