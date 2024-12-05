using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class GridManager1 : MonoBehaviour
{
    [SerializeField] private GameObject container;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private GameObject frame;


    void Start()
    {
        GenerateGrid();
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

        Debug.Log(positionY);
        Debug.Log(positionX);
        Vector3 startPosition = new Vector3(positionX, positionY);
        Vector3 rowStartPosition = startPosition;

        
        for (int i = 0; i < Mathf.Ceil(containerLocalScale.y); i++)
        {
            Vector3 position = rowStartPosition;
            for (int ii = 0; ii < Mathf.Ceil(containerLocalScale.x); ii++)
            {
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, container.transform);
                tile.transform.localPosition = position;
                position.x += newScaleX;
            }

            rowStartPosition.y -= newScaleY; 
        }
    }
}