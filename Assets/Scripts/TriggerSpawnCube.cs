using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TriggerSpawnCube : MonoBehaviour
{
    public InputActionReference activateReference = null;
    public GameObject cubePrefab;
    float value;
    GameObject cube;
    bool started = false;
    Vector3 startPosition;

    private void Update()
    {
        
        
        value = activateReference.action.ReadValue<float>();
        if (value == 1) { 
        
            
            if (!started)
            {
                startPosition = gameObject.transform.position;
                Activate();
                started = true;
            }
            
            if(started) {
                cube.transform.position = (startPosition + gameObject.transform.position) / 2f;
                cube.transform.localScale = startPosition - gameObject.transform.position;
            }
        }

        if (value == 0 && started)
        {
            started = false;
        }

    }

    private void Activate()
    {
        cube = Instantiate(cubePrefab, startPosition, Quaternion.Euler(gameObject.transform.eulerAngles));
    }
}
