using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerVideo : MonoBehaviour
{
    public RecordVideo recordVideo;
    public InputActionReference toggleReference = null;
    public GameObject UserDataGameObject;
    private UserData UserData;

    private void Awake()
    {
        toggleReference.action.started += Toggle;
        recordVideo = gameObject.AddComponent(typeof(RecordVideo)) as RecordVideo;
        
    }

    private void OnDestroy()
    {
        toggleReference.action.started -= Toggle;
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        var comp = UserDataGameObject.GetComponent<GetUserData>();
        UserData = comp.UserData;
        int value = (int)context.action.ReadValue<float>();
        
        recordVideo.UserData = UserData;
        recordVideo.StartVideo(value);
    }
}