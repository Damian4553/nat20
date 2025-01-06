using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField] private LayerMask gridLayer; // Layer mask for grid objects

    private int result;
    private MeasureArrow measureArrow;
    private MeasureFiledUiManager measureFiledUiManager;

    private bool isDifficultTerrain;
    private void OnMouseEnter()
    {
        measureArrow = GameObject.FindGameObjectWithTag("Manager").GetComponent<MeasureArrow>();
        measureFiledUiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MeasureFiledUiManager>();
        if (measureArrow.GetIsMeassureing())
        {
            Vector3 measureArrowStart = measureArrow.GetStart();
            Vector3 center = gameObject.GetComponent<Collider>().bounds.center;
            
            Ray ray = new Ray(measureArrowStart, (center - measureArrowStart).normalized);
            RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(measureArrowStart, center), gridLayer);
            
            RaycastHit[] difficultTerrainHits = hits.Where(hit => hit.collider.CompareTag("difficultTerrain")).ToArray();

            Debug.Log((hits.Length - difficultTerrainHits.Length)*5 );
            Debug.Log(difficultTerrainHits.Length*10);
            
            result = difficultTerrainHits.Length * 10 + (hits.Length - difficultTerrainHits.Length) * 5;
            
            measureFiledUiManager.UpdateText(result);
            
            measureArrow.AdjustObjectBetweenTargets(center);  // Call the function from ExampleClass
        }
    }

}
