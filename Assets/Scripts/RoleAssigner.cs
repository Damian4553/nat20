using System;
using Unity.Netcode;
using UnityEngine;

public class RoleAssignment : NetworkBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            var selectedRole = RoleManager.Instance.SelectedRole;
            // AssignRoleServerRpc(selectedRole);
        }
    }

    [ServerRpc]
    private void AssignRoleServerRpc(RoleManager.Role role)
    {
        AssignRoleClientRpc(role);
    }

    [ClientRpc]
    private void AssignRoleClientRpc(RoleManager.Role role)
    {
        GameSceneManager sceneManager = GameSceneManager.Instance;

        if (sceneManager == null)
        {
            Debug.LogError("GameSceneManager instance not found!");
            return;
        }

        // Fetch and apply visibility for the objects related to the assigned role
        var objectsToShow = sceneManager.GetObjectsForRole(role);
        var objectsToHide = role == RoleManager.Role.Player
            ? sceneManager.GetObjectsForRole(RoleManager.Role.Dm)
            : sceneManager.GetObjectsForRole(RoleManager.Role.Player);

        SetVisibility(objectsToShow, true);
        SetVisibility(objectsToHide, false);
    }

    private void SetVisibility(GameObject[] objects, bool isVisible)
    {
        if (objects == null) return;

        foreach (var obj in objects)
        {
            obj.SetActive(isVisible);
        }
    }

}