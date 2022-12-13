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
    private bool canPress;
    
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
        if (!canPress) return;
        movementScript = xrOrigin.GetComponent<ActionBasedContinuousMoveProvider>();
        movementScript.enabled = true;
        wheelForceActive = false;
    }
    
    public void WheelForceActive()
    {
        movementScript = xrOrigin.GetComponent<ActionBasedContinuousMoveProvider>();
        movementScript.enabled = false;
        wheelForceActive = true;
        canPress = true;
    }

    public void ToggleValue()
    {
        toggleForceValue = !toggleForceValue;
    }
}
