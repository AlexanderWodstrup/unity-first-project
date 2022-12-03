using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[FirestoreData]
public struct FirebaseGroups
{
    [FirestoreProperty] public string groupId { get; set; }

    [FirestoreProperty] public string groupName { get; set; }
}

[FirestoreData]
public struct FirebaseUser
{
    [FirestoreProperty] public string mail { get; set; }

    [FirestoreProperty] public string name { get; set; }

    [FirestoreProperty] public string pbPath { get; set; }

    [FirestoreProperty] public FirebaseGroups[] groups { get; set; }
}

public class SelectAProject : MonoBehaviour
{
    public UserData UserDataComponent;
    private FirebaseFirestore db;
    private FirebaseUser firebaseUser;
    public GameObject prefab; //place -40 Y then the last one
    [SerializeField] private Image prefabParrent;
    [SerializeField] private GameObject ProjectNaming;
    [SerializeField] private TextMeshProUGUI GroupName;
    private FirebaseGroups[] _firebaseGroups;


    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        getUserProjects();
        //Get user from login script
        //use the user from above to get firebase user
    }

    void getUserProjects()
    {
        db.Collection("users").Document(UserDataComponent.getUser().UserId).GetSnapshotAsync().ContinueWithOnMainThread(
            task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    firebaseUser = task.Result.ConvertTo<FirebaseUser>();
                    int yPos = 155;
                    _firebaseGroups = firebaseUser.groups;
                    foreach (var firebaseUserGroup in firebaseUser.groups)
                    {
                        Vector3 postion = prefabParrent.transform.position;

                        Text prefabText = prefab.GetComponentInChildren<Text>();
                        prefabText.text = firebaseUserGroup.groupName;

                        GameObject newToggle = Instantiate(prefab, prefabParrent.transform);
                        newToggle.name = "Toggle(" + firebaseUserGroup.groupName + ")";
                        var comp = newToggle.GetComponent<SetNamingScript>();
                        comp.ProjectNaming = ProjectNaming;
                        comp.GroupName = GroupName;
                        comp.UserDataComponent = UserDataComponent;
                        comp._FirebaseGroups = firebaseUserGroup;
                        newToggle.transform.localPosition = new Vector3(postion.x - 10, postion.y + yPos, postion.z);

                        yPos = yPos - 40;
                    }
                }
                else
                {
                    Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });
    }

    private string inputProjectName;
    private string inputDrawingName;

    public void beginDrawing()
    {
        UserDataComponent.storeProjectName(inputProjectName);
        UserDataComponent.storeDrawingName(inputDrawingName);

        StartCoroutine(LoadAsyncScene());
    }

    public void saveProjectName(string s)
    {
        inputProjectName = s;
    }

    public void saveDrawingName(string s)
    {
        inputDrawingName = s;
    }

    IEnumerator LoadAsyncScene()
    {
        Debug.Log("Completed - Loading new scene");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main VR Scene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}