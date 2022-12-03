using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerScreenshot : MonoBehaviour
{
    public PhotoCapture photoCapture;
    public InputActionReference toggleReference = null;
    public GameObject UserDataGameObject;
    private UserData UserData;

    private void Awake() 
    {
        toggleReference.action.started += Toggle;
        photoCapture = gameObject.AddComponent(typeof(PhotoCapture)) as PhotoCapture;
    }

    private void OnDestroy()
    {
        toggleReference.action.started -= Toggle;
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        var comp = UserDataGameObject.GetComponent<GetUserData>();
        UserData = comp.UserData;
        photoCapture.UserData = UserData;
        photoCapture.TakePicture();
    }
}
