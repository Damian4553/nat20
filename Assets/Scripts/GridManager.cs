using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject container;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private GameObject frame;
    
    [SerializeField] private LayerManager layerManager;

    public event Action OnGridResized;


    void Start()
    {
        GenerateGrid();

        layerManager.OnResize += Resize;
    }

    void GenerateGrid()
    {
        var containerLocalScale = container.transform.localScale;


        container.transform.localScale =
            new Vector3(Mathf.Ceil(containerLocalScale.x), Mathf.Ceil(containerLocalScale.y));


        float newScaleX = 1 / containerLocalScale.x;
        float newScaleY = 1 / containerLocalScale.y;

        tilePrefab.transform.localScale = new Vector3(newScaleX, newScaleY);

        float gridWidth = containerLocalScale.x * newScaleX;
        float gridHeight = containerLocalScale.y * newScaleY;
        float positionX = -gridWidth / 2 + newScaleX / 2;
        float positionY = gridHeight / 2 - newScaleY / 2;


        Vector3 startPosition = new Vector3(positionX, positionY);
        Vector3 rowStartPosition = startPosition;

        
        for (int row = 0; row < Mathf.Ceil(containerLocalScale.y); row++)
        {
            Vector3 position = rowStartPosition;
            for (int column = 0; column < Mathf.Ceil(containerLocalScale.x); column++)
            {
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                tile.transform.localPosition = position;
                position.x += newScaleX;
            }

            rowStartPosition.y -= newScaleY; 
        }
    }

    private void Resize(float x, float y)
    {
        Debug.Log(x);
        Debug.Log(y);
        // Update container scale
        container.transform.localScale = new Vector3(x, y);

        // Clear existing grid
        foreach (Transform child in container.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Grid"))
            {
                Destroy(child.gameObject);
            }
        }

        // Regenerate grid with the new scale
        GenerateGrid();

        OnGridResized?.Invoke();

    }


}