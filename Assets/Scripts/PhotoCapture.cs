using System;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;
using Object = UnityEngine.Object;

public class PhotoCapture : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageRef;
    public UserData UserData;
    public UpdateGroupFiles updateGroupFiles;
    
    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://wiumsverden.appspot.com");
    }

    public void TakePicture()
    {
        updateGroupFiles = gameObject.AddComponent(typeof(UpdateGroupFiles)) as UpdateGroupFiles;
        Start();
        string[] wildcardSplit = DateTime.Now.ToString().Split(" ");
        string wildcard = wildcardSplit[0].Replace("/","-") + " - " + wildcardSplit[1].Replace(":", "-") + " " + wildcardSplit[2];
        string filename = UserData.getDrawingName() + " - " + wildcard;
        string filetype = ".png";
        string filepath = Directory.GetCurrentDirectory() + "\\Image\\" + filename;
        
        if (storage != null && storageRef != null)
        {
            Debug.Log("Starting screen shot process");
            StartCoroutine(RecordFrame(filepath, filename + filetype));
        }
        else
        {
            Debug.Log("No firebase reffernce found");
        }
        
    }

    IEnumerator RecordFrame(string filepath, string fileNameAndType)
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture(3);
        
        // do something with texture
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filepath, bytes);
        
        // cleanup
        yield return new WaitUntil(() => File.Exists(filepath));
        Object.Destroy(texture);

        UploadPicture(filepath, fileNameAndType);
    }

    private void UploadPicture(string filePath, string fileNameAndType)
    {

        if (File.Exists(filePath))
        {
            // File located on disk
            string localFile = filePath;

            // Create a reference to the file you want to upload
            string firebaseFilePath = UserData.getFirebaseGroup().groupId + "/" + UserData.getProjectName() + "/" +
                                      "images/" + fileNameAndType;
            StorageReference riversRef = storageRef.Child(firebaseFilePath);

            // Upload the file to the path "images/rivers.jpg"
            riversRef.PutFileAsync(localFile)
                .ContinueWith((Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        // Metadata contains file metadata such as size, content-type, and download URL.
                        StorageMetadata metadata = task.Result;
                        string md5Hash = metadata.Md5Hash;
                        
                        updateGroupFiles.UserData = UserData;
                        
                        updateGroupFiles.AddNewFile(firebaseFilePath, fileNameAndType.Replace(".png", ""), ".png", UserData.getProjectName());
                        
                        
                        //Debug.Log("md5 hash = " + md5Hash);
                    }
                });
        }
    }
}