using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FaderMoveStartMenu : MonoBehaviour
{

    private GameObject applicationSettings;
    private ApplicationData applicationData;

    [SerializeField] private float upperPosBoundary = 0.0236f;
    [SerializeField] private float lowerPosBoundary = 0.06782f;
    [SerializeField] private float sensitivityY = 0.05f;

    public static PositionValueRelation[] faderPvr = FaderPvr.relation;
    public float value;
    public string channel;
    private float verticalMovement;

    private bool isMouseOverUI = false;

    private void Awake()
    {
        applicationSettings = GameObject.FindGameObjectWithTag("ApplicationSettings");
        if (applicationSettings == null)
        {
            applicationSettings = new GameObject("ApplicationSettings");
            applicationSettings.tag = "ApplicationSettings";
            applicationSettings.AddComponent<ApplicationData>();
        }
        applicationData = applicationSettings.GetComponent<ApplicationData>();

        var parent = transform;
        while (!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;
    }

    void Start()
    {
        SlideFader(0); // Initial "move" to get initial value from position
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { isMouseOverUI = true; }
        else { isMouseOverUI = false; }
    }

    private void OnMouseDrag()
    {
        // abort if mouse is over UI
        if (isMouseOverUI) { return; }

        SlideFader(Input.GetAxis("Mouse Y"));
    }

    float GetNonLinearFaderValue(PositionValueRelation[] pvr)
    {
        var pos = transform.localPosition.x;
        foreach (var relation in faderPvr)
        {
            if (pos >= relation.positions[0] && pos <= relation.positions[1])
            {
                return GetFaderValue(
                    relation.positions[0], relation.positions[1],
                    relation.values[0], relation.values[1]
                    );
            }
        }
        return 0f;
    }

    /**
     * Function calculates a value between 'min' and 'max'
     * based on the game objects position. This works if the
     * relation between scale and position is linear.
     * Otherwise function 'GetNonLinearFaderValue' ist needed.
     */
    float GetFaderValue(float upperBound, float lowerBound, float scaleMax, float scaleMin)
    {
        var pos = transform.localPosition.x;
        return (pos - lowerBound) * (scaleMax - scaleMin) / (upperBound - lowerBound) + scaleMin;

    }

    public static float GetNonLinearFaderPosition(float value)
    {
        foreach (var relation in faderPvr)
        {
            if (value <= relation.values[0] && value >= relation.values[1])
            {
                return GetFaderPosition(
                    relation.positions[0], relation.positions[1],
                    relation.values[0], relation.values[1],
                    value
                    );
            }
        }
        return 0f;
    }

    public static float GetFaderPosition(float upperBoundPos, float lowerBoundPos, float scaleMax, float scaleMin, float value)
    {
        return (value - scaleMin) * (upperBoundPos - lowerBoundPos) / (scaleMax - scaleMin) + lowerBoundPos;
    }



    private void SlideFader(float inputForce)
    {
        verticalMovement = inputForce * sensitivityY * Time.deltaTime;
        float posX = transform.localPosition.x - verticalMovement;
        float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
        transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
        value = GetNonLinearFaderValue(faderPvr);

        ChangeVolumeInSettings();
    }

    private void ChangeVolumeInSettings()
    {
        if (channel == "Master")
        {
            applicationData.masterVolume = (value + 80) / 90;
            applicationData.settingsPanel.masterSlider.value = applicationData.masterVolume;
        }
        else if (channel == "StereoInput1")
        {
            applicationData.systemVolume = (value + 80) / 90;
            applicationData.settingsPanel.systemSlider.value = applicationData.systemVolume;
        }
    }


}