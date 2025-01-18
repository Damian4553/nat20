using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Token : NetworkBehaviour
{
    [SerializeField] private Size selectedSize;

    public enum Size
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Gargantuan
    }

    void Start()
    {
        Resize(selectedSize);
    }

    public void Resize(Size size)
    {
        switch (size)
        {
            case Size.Tiny:
                gameObject.transform.localScale = new Vector3(.5f, .5f);
                break;
            case Size.Small:
                gameObject.transform.localScale = new Vector3(.75f, .75f);
                break;
            case Size.Medium:
                gameObject.transform.localScale = new Vector3(1f, 1f);
                break;
            case Size.Large:
                gameObject.transform.localScale = new Vector3(2f, 2f);
                break;
            case Size.Huge:
                gameObject.transform.localScale = new Vector3(3f, 3f);
                break;
            case Size.Gargantuan:
                gameObject.transform.localScale = new Vector3(4f, 4f);
                break;
            default:
                print("No size chosen");
                break;
        }

        SubmitPositionRequestServerRpc(gameObject.transform.localScale);
    }

    public void TextUpdate(string newText)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = newText;
    }
    
        
    [ServerRpc(RequireOwnership = false)]
    private void SubmitPositionRequestServerRpc(Vector3 newScale)
    {

        transform.localScale = newScale;
        UpdatePositionClientRpc(newScale);
    }
    
    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newScale)
    {
        // Synchronizacja pozycji na wszystkich klientach
        transform.localScale = newScale;
    }
}