using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

[FirestoreData]
public struct FirebaseGroupMember
{
    [FirestoreProperty] public string userId { get; set; }

    [FirestoreProperty] public string userName { get; set; }

    [FirestoreProperty] public string userRole { get; set; }
}

[FirestoreData]
public struct FirebaseGroupPost
{
    [FirestoreProperty] public string label { get; set; }

    [FirestoreProperty] public Timestamp timestamp { get; set; }
}

[FirestoreData]
public struct FirebaseGroupProjectFile
{
    [FirestoreProperty] public string filePath { get; set; }

    [FirestoreProperty] public string name { get; set; }

    [FirestoreProperty] public string type { get; set; }
}

[FirestoreData]
public struct FirebaseGroupProject
{
    [FirestoreProperty] public List<FirebaseGroupProjectFile> files { get; set; }

    [FirestoreProperty] public string name { get; set; }
}

[FirestoreData]
public struct FirebaseGroup
{
    [FirestoreProperty] public string description { get; set; }

    [FirestoreProperty] public List<FirebaseGroupMember> members { get; set; }

    [FirestoreProperty] public string name { get; set; }

    [FirestoreProperty] public string owner { get; set; }

    [FirestoreProperty] public List<FirebaseGroupPost> posts { get; set; }

    [FirestoreProperty] public List<FirebaseGroupProject> projects { get; set; }
}


public class UpdateGroupFiles : MonoBehaviour
{
    // Start is called before the first frame update
    private FirebaseFirestore db;

    //private DocumentReference docRef;
    public UserData UserData;
    private FirebaseGroup _firebaseGroup;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void AddNewFile(string filePath, string fileName, string fileType, string projectName)
    {
        db.Collection("groups").Document(UserData.getFirebaseGroup().groupId).GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    _firebaseGroup = task.Result.ConvertTo<FirebaseGroup>();
                    
                    FirebaseGroup newGroup = ChangeFirebaseGroupData(_firebaseGroup, filePath, fileName, fileType, projectName);
                    
                    db.Collection("groups").Document(UserData.getFirebaseGroup().groupId).SetAsync(newGroup);
                }
                else
                {
                    Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });
    }

    private FirebaseGroup ChangeFirebaseGroupData(FirebaseGroup group, string filePath, string fileName,
        string fileType, string projectName)
    {
        var newGroup = group;

        if (fileType == ".png")
        {
            fileType = "image";
        }
        else
        {
            fileType = "video";
        }

        try
        {
            var projectIndex = newGroup.projects.FindIndex((item) => item.name == projectName);

            newGroup.projects[projectIndex].files.Add(new FirebaseGroupProjectFile()
                { filePath = filePath, name = UserData.getDrawingName(), type = fileType });

            
        }
        catch (Exception e)
        {
            newGroup.projects.Add(new FirebaseGroupProject()
            {
                name = projectName,
                files = new List<FirebaseGroupProjectFile>()
                    { new FirebaseGroupProjectFile() { filePath = filePath, name = UserData.getDrawingName(), type = fileType } }
            });
        }
        
        newGroup.posts.Add(new FirebaseGroupPost(){label = UserData.getDrawingName() + " was uploaded from VR. Project: " +UserData.getProjectName() + ". Type: " + fileType, timestamp = Timestamp.GetCurrentTimestamp()});
        
        return newGroup;
    }
}