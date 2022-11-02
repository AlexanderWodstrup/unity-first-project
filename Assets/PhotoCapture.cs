using System.Collections;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;

public class PhotoCapture : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageRef;

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://wiumsverden.appspot.com");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Starting screen shot process");
            string filename = "SomePicture";
            string filetype = ".png";
            string filepath = Application.dataPath + "/" + filename + filetype;

            StartCoroutine(RecordFrame(filepath, filename + filetype));
        }
    }

    IEnumerator RecordFrame(string filepath, string fileNameAndType)
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        
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
            StorageReference riversRef = storageRef.Child("images/" + fileNameAndType);

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
                        Debug.Log("Finished uploading...");
                        //Debug.Log("md5 hash = " + md5Hash);
                    }
                });
        }
    }
}