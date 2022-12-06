using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelForce : MonoBehaviour
{
    public InputActionProperty leftHandMoveObject;
    private Vector2 leftHandValue;

    private MovementScriptToggle mvmScript;

    //private GameObject linkedTo;
    private List<GameObject> linkedTo;

    private void Start()
    {
        mvmScript = GameObject.Find("WristCanvas").GetComponent<MovementScriptToggle>();
        linkedTo = new List<GameObject>();
    }

    private void Update()
    {
        if (leftHandMoveObject.action.inProgress && mvmScript.wheelForceActive)
        {
            leftHandValue = leftHandMoveObject.action?.ReadValue<Vector2>() ?? Vector2.zero;
            
            if (leftHandValue.x is > 0 and < 1)
            {
                MoveForward();
            } else if (leftHandValue.x is < 0 and > -1)
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
        transform.Translate(mvmScript.sliderValue * Vector3.forward * Time.deltaTime);
        foreach (var link in linkedTo)
        {
            link.transform.Translate(mvmScript.sliderValue * Vector3.forward * Time.deltaTime);
        }
    }
    
    private void MoveBackward()
    {
        transform.Translate(mvmScript.sliderValue * Vector3.back * Time.deltaTime);
        foreach (var link in linkedTo)
        {
            link.transform.Translate(mvmScript.sliderValue * Vector3.back * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
    
        if (collision.collider.tag == "Collidable")
        {
            if (!linkedTo.Contains(collision.gameObject))
            {
                linkedTo.Add(collision.gameObject);
            }
            
            foreach (var link in linkedTo)
            {
                link.transform.rotation = transform.rotation;
            }
        }
    }
  
}
