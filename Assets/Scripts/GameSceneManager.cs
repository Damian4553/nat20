using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerObjects; // Objects visible to Role1

    [SerializeField] private GameObject[] dmObjects; // Objects visible to Role2
    
    private HashSet<GameObject> sharedObjects;
    public static GameSceneManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FindSharedObjects();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Optionally handle single-player role assignment directly here
        if (RoleManager.Instance.SelectedRole != RoleManager.Role.None)
        {
            AssignRole(RoleManager.Instance.SelectedRole);
        }
    }
    
    private void FindSharedObjects()
    {
        // Identify objects shared between Role1 and Role2
        sharedObjects = new HashSet<GameObject>();
        foreach (var obj in playerObjects)
        {
            if (ArrayContains(dmObjects, obj))
            {
                sharedObjects.Add(obj);
            }
        }
    }
    
    private bool ArrayContains(GameObject[] array, GameObject obj)
    {
        foreach (var element in array)
        {
            if (element == obj) return true;
        }
        return false;
    }


    private void AssignRole(RoleManager.Role role)
    {
        switch (role)
        {
            case RoleManager.Role.Player:
                Debug.Log("joined as player");
                SetVisibility(playerObjects, true);
                SetVisibility(dmObjects, false);
                break;

            case RoleManager.Role.Dm:
                SetVisibility(playerObjects, false);
                SetVisibility(dmObjects, true);
                break;

            default:
                Debug.LogError("Invalid role selected!");
                break;
        }
    }

    public GameObject[] GetObjectsForRole(RoleManager.Role role)
    {
        return role switch
        {
            RoleManager.Role.Player => playerObjects,
            RoleManager.Role.Dm => dmObjects,
            _ => null,
        };
    }

    private void SetVisibility(GameObject[] objects, bool isVisible)
    {
        foreach (var obj in objects)
        {
            if (!sharedObjects.Contains(obj)) // Skip shared objects
            {
                obj.SetActive(isVisible);
            }
        }
    }
}
