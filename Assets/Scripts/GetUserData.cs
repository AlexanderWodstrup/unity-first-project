using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUserData : MonoBehaviour
{
    private GameObject UserDataGameObject;

    public UserData UserData;
    // Start is called before the first frame update
    void Start()
    {
        UserDataGameObject = GameObject.Find("UserData");
        if (UserDataGameObject != null)
        {
            UserData = UserDataGameObject.GetComponent<UserData>();
            Destroy(UserDataGameObject);
        }
    }

    
}
