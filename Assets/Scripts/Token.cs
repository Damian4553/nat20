using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private Size selectedSize;
    private enum Size
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
        Resize();
    }

    private void Resize()
    {
        switch (selectedSize)
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
    }

    public void TextUpdate(string newText)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = newText;
    }
}