using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelForce : MonoBehaviour
{
    private WheelCollisionList wheelCollisionList;
    
    public InputActionProperty leftHandMoveObject;
    private Vector2 leftHandValue;

    private MovementScriptToggle mvmScript;
    
    private List<GameObject> linkedToLocal;

    private void Start()
    {
        mvmScript = GameObject.Find("WristCanvas").GetComponent<MovementScriptToggle>();
        wheelCollisionList = GameObject.Find("WristCanvas").GetComponent<WheelCollisionList>();
        linkedToLocal = new List<GameObject>();
    }

    private void Update()
    {
        if (leftHandMoveObject.action.inProgress && mvmScript.wheelForceActive)
        {
            leftHandValue = leftHandMoveObject.action?.ReadValue<Vector2>() ?? Vector2.zero;
            
            if (leftHandValue.y is > 0 and < 1)
            {
                MoveForward();
            } else if (leftHandValue.y is < 0 and > -1)
            {
                MoveBackward();
            } 
        } else if (mvmScript.toggleForceValue)
        {
            MoveForward();
        } else
        {
            leftHandValue = Vector2.zero;
            transform.Translate(0, 0, 0);
        }
    }

    private void MoveForward()
    {
        transform.Translate((mvmScript.sliderValue / 5f) * Vector3.forward * Time.deltaTime);
        foreach (var link in linkedToLocal)
        {
            if (link == null)
            {
                linkedToLocal.Remove(link);
                return;
            }
            link.transform.Translate((mvmScript.sliderValue / 5f) * Vector3.forward * Time.deltaTime);
        }
    }
    
    private void MoveBackward()
    {
        transform.Translate((mvmScript.sliderValue / 5f) * Vector3.back * Time.deltaTime);
        foreach (var link in linkedToLocal)
        {
            if (link == null)
            {
                linkedToLocal.Remove(link);
                return;
            }
            link.transform.Translate((mvmScript.sliderValue / 5f) * Vector3.back * Time.deltaTime);
        }
    }

    private bool rotationSet;
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Collidable"))
        {
            foreach (var link in linkedToLocal)
            {
                if (link == null)
                {
                    linkedToLocal.Remove(link);
                    return;
                }
            }
            
            if (!wheelCollisionList.linkedTo.Contains(collision.gameObject))
            {
                linkedToLocal.Add(collision.gameObject);
            }
            else if (!rotationSet)
            {
                transform.rotation = collision.transform.rotation; 
                rotationSet = true;
            }

            if (rotationSet)
            {
                foreach (var link in linkedToLocal)
                {
                    link.transform.rotation = transform.rotation;
                }
            }
            
            wheelCollisionList.AddToList(collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach (var link in linkedToLocal)
        {
            wheelCollisionList.linkedTo.Remove(link);
        }
    }
}
