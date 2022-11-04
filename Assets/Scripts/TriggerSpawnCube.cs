using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TriggerSpawnCube : MonoBehaviour
{
    public InputActionReference activateReference = null;
    public GameObject cubePrefab;
    private void Awake()
    {
        //spawn cube 
    }

    private void Update()
    {
        float value = activateReference.action.ReadValue<float>();
        if (value == 1)
        {
            Activate();
        }
    }

    private void Activate()
    {
        Vector3 newCube = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Instantiate(cubePrefab, newCube, Quaternion.Euler(gameObject.transform.eulerAngles));
    }
}
