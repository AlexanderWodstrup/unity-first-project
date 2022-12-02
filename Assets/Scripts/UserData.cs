using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    private Firebase.Auth.FirebaseUser _user;
    private FirebaseUser _firebaseUser;
    private string _projectName;
    private string _drawingName;
    private FirebaseGroups _group;

    public void storeUser(Firebase.Auth.FirebaseUser newUser)
    {
        _user = newUser;
    }

    public Firebase.Auth.FirebaseUser getUser()
    {
        if (_user == null)
        {
            return null;
        }
        
        return _user;
    }

    public void storeFirebaseUser(FirebaseUser user)
    {
        _firebaseUser = user;
    }
    
    public FirebaseUser getFirebaseUser()
    {
        return _firebaseUser;
    }

    public void storeProjectName(string projectName)
    {
        _projectName = projectName;
    }

    public void storeDrawingName(string drawingName)
    {
        _drawingName = drawingName;
    }

    public void storeFirebaseGroup(FirebaseGroups group)
    {
        _group = group;
    }

    public string getProjectName()
    {
        return _projectName;
    }

    public string getDrawingName()
    {
        return _drawingName;
    }

    public FirebaseGroups getFirebaseGroup()
    {
        return _group;
    }
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
