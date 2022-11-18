using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetEmail : MonoBehaviour
{
    private string input;
    public void readString(string s)
    {
        input = s;
        Debug.Log("" + input);
    }
}
