using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class GetString : MonoBehaviour
{
    public UserData UserDataComponent;
    private string inputEmail;
    private string inputPassword;
    private FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser newUser;
    private bool isLoading = true;
    private bool isError = false;
    private bool isComplete = false;

    [SerializeField] private TextMeshProUGUI ErrorMsg;
    [SerializeField] private Canvas NextCanvas;

    public void saveEmail(string s)
    {
        inputEmail = s;
        
    }
    
    public void savePassword(string s)
    {
        inputPassword = s;
        
    }

    public void login()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(inputEmail, inputPassword).ContinueWith(task =>
        {
            isLoading = true;
            isError = false;
            isComplete = false;
            if (task.IsCanceled)
            {
                isError = true;
                isComplete = false;
                isLoading = false;
                //Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
        
            if (task.IsFaulted)
            {
                isError = true;
                isComplete = false;
                isLoading = false;
                
                //Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
        
            newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        
            isError = false;
            isComplete = true;
            isLoading = false;
        });
        StartCoroutine(LoadAsyncScene());
    }
    
    IEnumerator LoadAsyncScene()
    {
        
        yield return new WaitUntil(() => isLoading == false);

        if (isError == true)
        {
            Debug.Log("Error - Not loading new scene");
            ErrorMsg.SetText("Email or Password is incorrect - try agian");
            ErrorMsg.gameObject.SetActive(true);
            yield return null;
        }

        if (isComplete == true)
        {
            Debug.Log("Completed - Loading new scene");
            gameObject.SetActive(false);
            
            UserDataComponent.storeUser(newUser);
            NextCanvas.gameObject.SetActive(true);
            
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main VR Scene");
            //
            // // Wait until the asynchronous scene fully loads
            // while (!asyncLoad.isDone)
            // {
            //     yield return null;
            // }
        }
    }
}
