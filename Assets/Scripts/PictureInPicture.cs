using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // Executes in Edit Mode
public class PictureInPicture : MonoBehaviour
{
    public float pipWidth = 230f;   // width in px
    public float pipHeight = 484f;  // height in px
    private float pipAspectRatio;   // ratio between width/height
    public float pipRight = 25f;    // offset from right
    public float pipTop = 235f;     // offset from top

    private Camera pictureInPictureCamera;
    private int previousScreenWidth;
    private int previousScreenHeight;

    void Start()
    {
        pictureInPictureCamera = gameObject.GetComponent<Camera>();
        previousScreenWidth = Screen.width;
        previousScreenHeight = Screen.height;
    }
    void Update()
    {
        AdaptPipRect();
    }

    /**
     * This function will adapt the shown rectangle of the pip camera 
     * everytime the resolution changes. It will adapt the physical size 
     * as well as its position, so it always looks the same.
     * 
     * >> NOTE: IF YOU NEED TO CHANGE POSITION OR SIZE CHOOSE 'FULL HD' 
     *          RESOLUTION IN EDITOR! FULL HD IS THE RESOLUTION THESE 
     *          CALCULATIONS ARE BASED ON!
     */
    private void AdaptPipRect()
    {
        if (ScreenResolutionChanged())
        {
            // Start (based on FullHD)
            pipAspectRatio = pipWidth / pipHeight;
            var screenHeightTo1080Ratio = Screen.height / 1080f;

            // Calculate new resolution and position
            var pipHeightNew = pipHeight * screenHeightTo1080Ratio;
            var pipWidthNew = pipAspectRatio * pipHeightNew;

            var pipNewX = Screen.width - pipWidthNew - pipRight * screenHeightTo1080Ratio;
            var pipNewY = Screen.height - pipHeightNew - pipTop * screenHeightTo1080Ratio;

            pictureInPictureCamera.pixelRect = new Rect(pipNewX, pipNewY, pipWidthNew, pipHeightNew);
        }
    }

    private bool ScreenResolutionChanged()
    {
        if (previousScreenWidth != Screen.width || previousScreenHeight != Screen.height)
        {
            previousScreenWidth = Screen.width;
            previousScreenHeight = Screen.height;
            return true;
        }
        return false;
    }
}