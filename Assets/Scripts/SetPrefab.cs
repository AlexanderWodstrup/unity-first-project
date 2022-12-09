using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPrefab : MonoBehaviour
{
    public GameObject prefab;
    public bool toggled;
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    public GameObject wheelPrefab;
    [SerializeField] private GameObject cubeHand;
    [SerializeField] private GameObject sphereHand;
    [SerializeField] private GameObject wheelHand;
    [SerializeField] private GameObject eraser;
    [SerializeField] private GameObject hand;

    public void ChooseEraser()
    {
        if (!eraser.activeSelf)
        {
            prefab = null;
            CleanPrefabs();
            eraser.SetActive(true);
            
   
        }
        else
        {
            eraser.SetActive(false);
            hand.SetActive(true);
        }
    }

    private void CleanPrefabs()
    {
        prefab = null;
        eraser.SetActive(false);
        cubeHand.SetActive(false);
        sphereHand.SetActive(false);
        wheelHand.SetActive(false);
        hand.SetActive(false);
    }
    
    public void ChooseCube()
    {
        if (prefab == cubePrefab)
        {
            CleanPrefabs();
            hand.SetActive(true);
            prefab = null;
            toggled = false;
        }
        else
        {
            CleanPrefabs();
            cubeHand.SetActive(true);
            toggled = true;
            prefab = cubePrefab;
        }
    }
    
    public void ChooseSphere()
    {
        if (prefab == spherePrefab)
        {
            CleanPrefabs();
            hand.SetActive(true);
            prefab = null;
            toggled = false;
        }
        else
        {
            CleanPrefabs();
            sphereHand.SetActive(true);
            toggled = true;
            prefab = spherePrefab;
        }
    }
    
    public void ChooseWheel()
    {
        if (prefab == wheelPrefab)
        {
            CleanPrefabs();
            hand.SetActive(true);
            prefab = null;
            toggled = false;
        }
        else
        {
            CleanPrefabs();
            wheelHand.SetActive(true);
            toggled = true;
            prefab = wheelPrefab;
        }
    }
}
