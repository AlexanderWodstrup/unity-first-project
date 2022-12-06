using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerSpawnPrefab : MonoBehaviour
{
    public InputActionReference activateReference = null;
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public GameObject wheelPrefab;

    private GameObject prefab;

    public XRRayInteractor rightRay;
    
    float value;
    GameObject handPrefab;
    bool started = false;
    Vector3 startPosition;
    private bool toggled;

    private void Update()
    {
        bool isRightRayHovering = rightRay.TryGetCurrentUIRaycastResult(out RaycastResult raycastResult);

        value = activateReference.action.ReadValue<float>();
        if (value == 1 && !isRightRayHovering && toggled) { 
        
            
            if (!started)
            {
                startPosition = gameObject.transform.position;
                Activate();
                started = true;
            }
            
            if(started && prefab == cubePrefab || prefab == spherePrefab) {
                handPrefab.transform.position = (startPosition + gameObject.transform.position) / 2f;
                handPrefab.transform.localScale = startPosition - gameObject.transform.position;
            }
            
            if(started && prefab != cubePrefab || prefab != spherePrefab)
            {
                handPrefab.transform.position = gameObject.transform.position;
                handPrefab.transform.rotation = gameObject.transform.rotation;
            }
        }

        if (value == 0 && started)
        {
            started = false;
        }

    }

    private void Activate()
    {
        if (prefab == cubePrefab || prefab == spherePrefab)
        {
            handPrefab = Instantiate(prefab, startPosition, Quaternion.Euler(gameObject.transform.eulerAngles));
        }
        else if (prefab == wheelPrefab)
        {
            Vector3 newWheel = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            handPrefab = Instantiate(prefab, newWheel, Quaternion.Euler(gameObject.transform.eulerAngles));
        } 
    }
    
    public void ChooseCube()
    {
        if (prefab == cubePrefab)
        {
            prefab = null;
            toggled = false;
        }
        else
        {
            toggled = true;
            prefab = cubePrefab;
        }
    }
    
    public void ChooseSphere()
    {
        if (prefab == spherePrefab)
        {
            prefab = null;
            toggled = false;
        }
        else
        {
            toggled = true;
            prefab = spherePrefab;
        }
    }
    
    public void ChooseWheel()
    {
        if (prefab == wheelPrefab)
        {
            prefab = null;
            toggled = false;
        }
        else
        {
            toggled = true;
            prefab = wheelPrefab;
        }
    }
}