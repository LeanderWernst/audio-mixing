using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureInPicture : MonoBehaviour
{
    public Camera pictureInPictureCamera;
    public int pipWidth = 234;
    public int pipHeight = 484;
    public int offsetX = 24;
    public int offsetY = 362;
    public float scale = 1.0f;
    private float previousAspectRatio;

    void Start()
    {
        previousAspectRatio = GetAspectRatio();
    }


    //This will keep the pip camera frame with the same dimensions
    // not depending on changing aspect ratios
    void Update()
    {
        float currentAspectRatio = GetAspectRatio();
        // only change pip size when aspect ratio changes
        if (previousAspectRatio != currentAspectRatio)
        {
            float aspectRatio = (float)pipWidth / (float)pipHeight;
            pipWidth = (int)(scale * pipWidth);
            pipHeight = (int)(pipWidth / aspectRatio);
            pictureInPictureCamera.pixelRect = new Rect(Screen.width - pipWidth - offsetX, offsetY, pipWidth, pipHeight);
            previousAspectRatio = currentAspectRatio;
        }
    }

    private float GetAspectRatio()
    {
        return Screen.width / (float)Screen.height;
    }
}