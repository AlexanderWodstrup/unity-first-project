using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erase : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collidable") || collision.gameObject.CompareTag("Wheel"))
        {
            Destroy(collision.gameObject);
        }
    }
}
