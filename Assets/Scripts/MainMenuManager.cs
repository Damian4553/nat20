using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playerButton;
    [SerializeField] private Button dmButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start()
    {
        playerButton.onClick.AddListener(SelectPlayerRole);
        dmButton.onClick.AddListener(SelectDmRole);
        hostButton.onClick.AddListener(SelectHost);
        clientButton.onClick.AddListener(SelectClient);
    }

    private void SelectHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void SelectClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void SelectPlayerRole()
    {
        LoadGameScene();
        RoleManager.Instance.SetRole(RoleManager.Role.Player);
    }

    private void SelectDmRole()
    {
        LoadGameScene();
        RoleManager.Instance.SetRole(RoleManager.Role.Dm);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}