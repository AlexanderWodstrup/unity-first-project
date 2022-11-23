using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    private Firebase.Auth.FirebaseUser _user;
    private FirebaseUser _firebaseUser;

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
}
