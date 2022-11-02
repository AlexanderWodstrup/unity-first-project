using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleButtonTest : MonoBehaviour
{
    public PhotoCapture photoCapture;
    public InputActionReference toggleReference = null;

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
        //bool isActive = !gameObject.activeSelf;
        //gameObject.SetActive(isActive);
        //Debug.Log("Button: " + isActive);
        photoCapture.TakePicture();
        //gameObject.SetActive(!isActive);
        //Debug.Log("Button: " + !isActive);
        
    }
}
