using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour
{
    [SerializeField] private LayerMask tokenLayerMask;
    [SerializeField] private Transform tokenLayerTransform;
    [SerializeField] private Transform gmLayerTransform;

    private Camera mainCamera;
    
    public event Action OnLayerChanged;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PerformResize(Token.Size.Tiny);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PerformResize(Token.Size.Small);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PerformResize(Token.Size.Medium);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PerformResize(Token.Size.Large);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PerformResize(Token.Size.Huge);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            PerformResize(Token.Size.Gargantuan);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeLayer(tokenLayerTransform);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeLayer(gmLayerTransform);
        }
    }

    private void PerformResize(Token.Size size)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if ((tokenLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                hit.collider.gameObject.GetComponent<Token>().Resize(size);
            }
        }
        
    }

    // private void ChangeLayer(Transform newParentLayerTransform)
    // {
    //     Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //
    //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    //
    //     if (hit.collider != null)
    //     {
    //         if ((tokenLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
    //         {
    //             hit.collider.gameObject.GetComponent<Token>().transform.parent = newParentLayerTransform;
    //         }
    //     }
    // }
    
    private void ChangeLayer(Transform newParentLayerTransform)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if ((tokenLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                hit.collider.gameObject.GetComponent<Token>().transform.parent = newParentLayerTransform;
                
                // Notify the LayerManager of the layer change
                OnLayerChanged?.Invoke();
            }
        }
    }

}
