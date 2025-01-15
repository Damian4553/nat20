using UnityEngine;
using TMPro;

public class CreateToken : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnLayerTransform;

    private int currentNumber = 1;
    private Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPrefabAtClick();
        }
    }

    void SpawnPrefabAtClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Instantiate(prefab, hit.point, Quaternion.identity, spawnLayerTransform).GetComponent<Token>()
                .TextUpdate("T" + currentNumber);

            currentNumber++;
        }
    }
}