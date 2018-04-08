using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController: MonoBehaviour {
    [SerializeField]
    Image statBar;
    float fillAmount;
    float value;
    public float Value
    {
        set
        {
            this.value = value;
            fillAmount = FindFill(value, minVal, maxVal, 0, 1);
            HandleBar();
        }
    }
    float maxVal;
    public float MaxVal
    {
        set
        {
            maxVal  = value;
            fillAmount = FindFill(this.value, minVal, maxVal, 0, 1);
            HandleBar();
        }
    }
    float minVal;
    public float MinVal
    {
        set
        {
            minVal = value;
            fillAmount = FindFill(this.value, minVal, maxVal, 0, 1);
            HandleBar();
        }
    }
	void Start () {
        
	}
	
	void Update () {
	}
    void HandleBar()
    {
        /*Debug.Log(fillAmount);
        Debug.Log(minVal);
        Debug.Log(maxVal);
        Debug.Log(value);*/
        if (statBar.fillAmount != fillAmount)
        {
            statBar.fillAmount = fillAmount;
        }
    }
    float FindFill(float value, float barMin, float barMax, float fillMin, float fillMax)
    {
        float ret;
        ret = ((value - barMin) / (barMax - barMin)) * (fillMax-fillMin) + fillMin;
        return ret;
    } 
}
