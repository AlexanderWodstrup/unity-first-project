using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;

    private Texture2D screenCapture;
    public int fileCounter;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            //TakeScreenshot
            StartCoroutine(CapturePhote());
        }
    }

    IEnumerator CapturePhote()
    {
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        var bytes = screenCapture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + fileCounter + ".png", bytes);
        fileCounter++;



        ShowPhoto();
    }

    void ShowPhoto()
    {
        Sprite photeSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photeSprite;
    }
}
