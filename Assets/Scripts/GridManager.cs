using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{

    [SerializeField] private GameObject container;
    [SerializeField] private GameObject lastColumnContainer;
    
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilePrefabIgnoreLayout;

    [SerializeField] private GameObject frame;

    
    
    void Start()
    {
        GenerateGrid();
    }
    
    void GenerateGrid()
    {

        var containerLocalScale = container.transform.localScale;
        
        
        
        container.transform.localScale = new Vector3(Mathf.Ceil(containerLocalScale.x),Mathf.Ceil(containerLocalScale.y));
        
        float numberOfSquares = Mathf.Floor(containerLocalScale.x) * Mathf.Ceil(containerLocalScale.y);

        float newScaleX = 1 / containerLocalScale.x;
        float newScaleY = 1 / containerLocalScale.y;
        container.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(newScaleX, newScaleY);

        tilePrefab.transform.localScale = new Vector3(newScaleX, newScaleY);
        tilePrefabIgnoreLayout.transform.localScale = new Vector3(newScaleX, newScaleY);
        
        for (int i = 0; i < numberOfSquares; i++)
        {
            Instantiate(tilePrefab, new Vector3(0, 0), Quaternion.identity,container.transform);
        }

        if (Mathf.Floor(containerLocalScale.x) < containerLocalScale.x)
        {
            Vector3 position =  new Vector3(0, (float)(-0.5*containerLocalScale.y));
            
            for (int i = 0; i < containerLocalScale.x; i++)
            {
                Debug.Log(position);
                Instantiate(tilePrefabIgnoreLayout, new Vector3(0, 0), Quaternion.identity, lastColumnContainer.transform);
                position += new Vector3(0, newScaleY);
            }
        }

    }

}
