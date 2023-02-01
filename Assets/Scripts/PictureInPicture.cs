using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode] // Executes in Edit Mode
public class PictureInPicture : MonoBehaviour
{
    [SerializeField] private float pipWidth = 230f;   // width in px
    [SerializeField] private float pipHeight = 484f;  // height in px
    [SerializeField] private float pipTop = 235f;     // offset from top
    [SerializeField] private float pipRightVisible = 25f; // offset from right
    [SerializeField] private bool isHidden = true;
    private float pipRightHidden;
    private float pipRight => isHidden ? pipRightHidden : pipRightVisible;    // offset from right
    private float pipAspectRatio;                     // ratio between width/height

    private Camera pictureInPictureCamera;
    private int previousScreenWidth;
    private int previousScreenHeight;

    private float screenHeightTo1080Ratio;
    private float pipHeightNew;
    private float pipWidthNew;
    private float pipNewX;
    private float pipNewY;

    private Coroutine coroutine;

    void Start()
    {
        pictureInPictureCamera = gameObject.GetComponent<Camera>();
        pictureInPictureCamera.enabled = !isHidden;
        previousScreenWidth = 1920;
        previousScreenHeight = 1080;
        pipRightHidden = -pipWidth;
        AdaptPipRect(); // initial adaption
    }
    void Update()
    {
        // Only adapt rectangle, when resolution changes
        if (ScreenResolutionChanged() || UnityIsInEditMode())
        { 
            AdaptPipRect();
        }
    }

    /**
     * This function will adapt the shown rectangle of the pip camera 
     * It will adapt the physical size as well as its position, so it 
     * always looks the same.
     * 
     * >> NOTE: IF YOU NEED TO CHANGE POSITION OR SIZE CHOOSE 'FULL HD' 
     *          RESOLUTION IN EDITOR! FULL HD IS THE RESOLUTION THESE 
     *          CALCULATIONS ARE BASED ON!
     */
    private void AdaptPipRect()
    {
        // Start (based on FullHD)
        pipAspectRatio = pipWidth / pipHeight;
        screenHeightTo1080Ratio = Screen.height / 1080f;

        // Calculate new resolution and position
        pipHeightNew = pipHeight * screenHeightTo1080Ratio;
        pipWidthNew = pipAspectRatio * pipHeightNew;

        pipNewX = Screen.width - pipWidthNew - pipRight * screenHeightTo1080Ratio;
        pipNewY = Screen.height - pipHeightNew - pipTop * screenHeightTo1080Ratio;

        pictureInPictureCamera.pixelRect = new Rect(pipNewX, pipNewY, pipWidthNew, pipHeightNew);
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

    private bool UnityIsInEditMode()
    {
        // Compile depending on environment
        #if UNITY_EDITOR
            return !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode;
        #else
            return false;
        #endif
    }

    /**
     * This function toggles the pip camera to slide in or out the frame
     */
    public void ToggleSmoothSlide(float duration = 2f)
    {
        isHidden = !isHidden;
        if (!isHidden)
        {
            pictureInPictureCamera.enabled = true;
        }
        float pipX;
        if (isHidden)
        {
            pipX = Screen.width - pipWidthNew - pipRightHidden * screenHeightTo1080Ratio;
        }
        else
        {
            pipX = Screen.width - pipWidthNew - pipRight * screenHeightTo1080Ratio;
        }
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SmoothSlidePipToNewX(pipX, duration));
    }

    IEnumerator SmoothSlidePipToNewX(float targetPipX, float duration)
    {
        float startX = pipNewX;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
            t = EaseInOut(t);
            //t = EaseInOutCubic(t);
            pipNewX = Mathf.Lerp(startX, targetPipX, t);
            pictureInPictureCamera.pixelRect = new Rect(pipNewX, pipNewY, pipWidthNew, pipHeightNew);
            yield return null;
        }
        if (isHidden)
        {
            pictureInPictureCamera.enabled = false;
        }
    }

    private float EaseInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x* x* x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    private float EaseInOut(float x)
    {
        return (Mathf.Sin((x - 0.5f) * Mathf.PI) + 1) / 2;
    }

    public bool IsVisible()
    {
        return !isHidden;
    }
}