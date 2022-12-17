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

    public XRRayInteractor rightRay;
    
    float value;
    GameObject handPrefab;
    bool started = false;
    Vector3 startPosition;

    [SerializeField] private SetPrefab prefabScript;

    private void Update()
    {

        bool isRightRayHovering = rightRay.TryGetCurrentUIRaycastResult(out RaycastResult raycastResult);

        value = activateReference.action.ReadValue<float>();
        if (value == 1 && !isRightRayHovering && prefabScript.toggled) { 
        
            
            if (!started)
            {
                startPosition = transform.InverseTransformPoint(gameObject.transform.position);
                Activate();
                started = true;
            }
            
            if(started && prefabScript.prefab == prefabScript.cubePrefab || prefabScript.prefab == prefabScript.spherePrefab) {
                handPrefab.transform.position = (startPosition + transform.InverseTransformPoint(gameObject.transform.position)) / 2f;
                handPrefab.transform.localScale = startPosition - transform.InverseTransformPoint(gameObject.transform.position);
            }
        }

        if (value == 0 && started)
        {
            started = false;
        }

    }

    private void Activate()
    {
        if (prefabScript.prefab == prefabScript.cubePrefab || prefabScript.prefab == prefabScript.spherePrefab)
        {
            handPrefab = Instantiate(prefabScript.prefab, startPosition, Quaternion.Euler(gameObject.transform.eulerAngles));
        }
        else if (prefabScript.prefab == prefabScript.wheelPrefab)
        {
            // Vector3 newWheel = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            // handPrefab = Instantiate(prefabScript.prefab, newWheel, Quaternion.Euler(gameObject.transform.eulerAngles));
            handPrefab = Instantiate(prefabScript.prefab, transform.InverseTransformPoint(gameObject.transform.position), Quaternion.Euler(gameObject.transform.eulerAngles));
        } 
    }
}