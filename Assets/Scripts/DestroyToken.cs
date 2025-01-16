using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyToken : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectAndDestroyPrefab();
        }
    }

    void DetectAndDestroyPrefab()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        
        if (hit.collider != null)
        {
            if ((layer.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
