using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasureButton : MonoBehaviour
{
    [SerializeField] private Button cursorButton;
    [SerializeField] private Button measureButton;
    // Start is called before the first frame update
    void Start()
    {
        cursorButton.onClick.AddListener(TurnOffMeasure);
        measureButton.onClick.AddListener(TurnOnMeasure);
    }

    private void TurnOffMeasure()
    {
        gameObject.GetComponent<MeasureArrow>().DestroyArrow();
        gameObject.GetComponent<MeasureArrow>().enabled = false;
        
        gameObject.GetComponent<MeasureFiledUiManager>().DestroyCanvas();
        gameObject.GetComponent<MeasureFiledUiManager>().enabled = false;
    }
    private void TurnOnMeasure()
    {
        gameObject.GetComponent<MeasureArrow>().enabled = true;
        
        gameObject.GetComponent<MeasureFiledUiManager>().enabled = true;
    }
}
