using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementScriptToggle : MonoBehaviour
{
    private GameObject xrOrigin;
    private ActionBasedContinuousMoveProvider movementScript;
    
    public bool wheelForceActive;
    
    //Slider value
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    public float sliderValue;
    
    public bool toggleForceValue;
    
    private void Start()
    {
        xrOrigin = GameObject.Find("XR Origin");
        movementScript = xrOrigin.GetComponent<ActionBasedContinuousMoveProvider>();
        
        slider.onValueChanged.AddListener(val =>
        {
            sliderText.text = val.ToString("0.00");
            sliderValue = val;
        });
    }

    public void MovementActive()
    {
        movementScript.enabled = true;
        wheelForceActive = false;
    }
    
    public void WheelForceActive()
    {
        movementScript.enabled = false;
        wheelForceActive = true;
    }

    public void ToggleValue()
    {
        toggleForceValue = !toggleForceValue;
        Debug.Log("toggle " + toggleForceValue);
    }
}
