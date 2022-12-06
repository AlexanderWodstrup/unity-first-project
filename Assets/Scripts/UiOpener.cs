using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiOpener : MonoBehaviour
{
    public InputActionProperty showPrefabUI;
    public InputActionProperty showMovementUI;
    public GameObject prefabCanvas;
    public GameObject movementCanvas;
    
    void Update()
    {
        if (showPrefabUI.action.WasPressedThisFrame())
        {
            OpenPrefabCanvas();
        }    
        if (showMovementUI.action.WasPressedThisFrame())
        {
            OpenMovementCanvas();
        }
    }

    private void OpenPrefabCanvas()
    {
        bool isActive = prefabCanvas.activeSelf;
        movementCanvas.SetActive(false);
        prefabCanvas.SetActive(!isActive);
    }
    
    private void OpenMovementCanvas()
    {
        bool isActive = movementCanvas.activeSelf;
        prefabCanvas.SetActive(false);
        movementCanvas.SetActive(!isActive);
    }
    
}
