using UnityEngine;

public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance { get; private set; }

    public enum Role { None, Player, Dm }

    private Role selectedRole = Role.None; // Store the selected role privately
    public Role SelectedRole => selectedRole; // Public getter for selectedRole

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void SetRole(Role role)
    {
        selectedRole = role;
    }
}