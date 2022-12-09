using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCollisionList : MonoBehaviour
{
    public List<GameObject> linkedTo;
    
    private void Start()
    {
        linkedTo = new List<GameObject>();
    }

    public void AddToList(GameObject link)
    {
        if (!linkedTo.Contains(link))
        {
            print("added");
            linkedTo.Add(link);
        }
    }

    public void RemoveFromList(GameObject link)
    {
        linkedTo.Remove(link);
    }
}
