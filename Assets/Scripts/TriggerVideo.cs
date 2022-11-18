using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerVideo : MonoBehaviour
{
    public RecordVideo recordVideo;
    public InputActionReference toggleReference = null;

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
        int value = (int)context.action.ReadValue<float>();
        Debug.Log(value);
        recordVideo.StartVideo(value);
    }
}