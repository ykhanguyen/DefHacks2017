using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.WebCam;
using System.Linq;
//using Windows.Storage;
using System;
using System.IO;

public class PhotoCaptureTest: MonoBehaviour
{
 
        PhotoCapture photoCaptureObject = null;
        void Start()
        {
            //PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        }

    public void TakePhoto()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated); 
        //Put into emotion_finder

        //Display emotion
    }

        void OnPhotoCaptureCreated(PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;

            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = cameraResolution.width;
            c.cameraResolutionHeight = cameraResolution.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;

            captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);    // Should be a false between c and OnPhotoModeStarted
        }
        void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
        private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
                string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);

                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            }
            else
            {
                Debug.LogError("Unable to start photo mode!");
            }
        }
        void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                Debug.Log("Saved Photo to disk!");
                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            }
            else
            {
                Debug.Log("Failed to save Photo to disk");
            }
        }
  
}