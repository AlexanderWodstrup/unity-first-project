using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Assets.OVR.Scripts;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class RecordVideo : MonoBehaviour
{
    private RecorderController _mRecorderController;
    private MovieRecorderSettings videoRecorder;
    private RecorderControllerSettings controllerSettings;
    FirebaseStorage storage;
    StorageReference storageRef;
    private string filePath;
    private string fileName;
    public UserData UserData;
    public UpdateGroupFiles updateGroupFiles;
    
    public void StartVideo(int value)
    {
        updateGroupFiles = gameObject.AddComponent(typeof(UpdateGroupFiles)) as UpdateGroupFiles;
        if (value == 1)
        {
            if (_mRecorderController.IsRecording() == false)
            {
                Debug.Log("Starting recording");
                Start();
                Record();
            }
            else
            {
                Debug.Log("Stopping recording");
                Stop();
                
            }
        }
    }
    
    private void Stop()
    {
        StartCoroutine(WaitForVideoFileToBeProcessed());
    }

    IEnumerator WaitForVideoFileToBeProcessed()
    {
        _mRecorderController.StopRecording();
        yield return new WaitForSeconds(3);
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        UploadVideo();
    }
    
    private void Record()
    {
        string[] wildcardSplit = DateTime.Now.ToString().Split(" ");
        string wildcard = wildcardSplit[0].Replace("/","-") + " - " + wildcardSplit[1].Replace(":", "-") + " " + wildcardSplit[2];
        fileName = UserData.getDrawingName() + " - " + wildcard;
        filePath = Directory.GetCurrentDirectory() + "\\Video\\" + fileName;
        videoRecorder.OutputFile = filePath;
        filePath = filePath + ".mp4";
        
        // Setup Recording
        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.FrameRate = 60;
        
        RecorderOptions.VerboseMode = false;
        _mRecorderController.PrepareRecording();
        _mRecorderController.StartRecording();
        Debug.Log("Recording status: " + _mRecorderController.IsRecording());
    }

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://wiumsverden.appspot.com");
        
        controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        _mRecorderController = new RecorderController(controllerSettings);
        
        // Image sequence
        videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "My Video Recorder";
        videoRecorder.Enabled = true;
        videoRecorder.VideoBitRateMode = VideoBitrateMode.High;

        videoRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputHeight = 1920,
            OutputWidth = 3664
        };

        videoRecorder.AudioInputSettings.PreserveAudio = false;
        
    }
    
    private void UploadVideo()
    {
        if (File.Exists(filePath))
        {
            // File located on disk
            string localFile = filePath;

            // Create a reference to the file you want to upload
            string firebaseFilePath = UserData.getFirebaseGroup().groupId + "/" + UserData.getProjectName() + "/" +
                                      "videos/" + fileName + ".mp4";
            StorageReference riversRef = storageRef.Child(firebaseFilePath);

            var newMetadata = new MetadataChange();
            newMetadata.ContentType = "video/mp4";

            riversRef.PutFileAsync(localFile, newMetadata, null, CancellationToken.None).ContinueWithOnMainThread((Task<StorageMetadata> task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log("Fuck");
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    
                    updateGroupFiles.UserData = UserData;
                        
                    updateGroupFiles.AddNewFile(firebaseFilePath, fileName, ".mp4", UserData.getProjectName());

                    Debug.Log("Finished uploading...");
                    //Debug.Log("md5 hash = " + md5Hash);
                }
            });
            
            
        }
    }

    
}