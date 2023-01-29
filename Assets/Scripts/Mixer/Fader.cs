using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Fader : MonoBehaviour
{

    [SerializeField] private float upperPosBoundary = 0.0236f;
    [SerializeField] private float lowerPosBoundary = 0.06782f;
    [SerializeField] private float sensitivityY = 0.05f;
    
    private PositionValueRelation[] faderPvr = FaderPvr.relation;
    public float value;
    private ValueStorage valueStorage;
    private float verticalMovement;
    public bool isClicked = false;
    public AudioController audioController;
    TextMeshProUGUI canvasValueText;
    public string channel;

    private bool isMouseOverUI = false;

    private List<string> blockedChannels = new List<string> { "Channel1", "Channel2", "Channel3" };
    private InteractionManager im;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        valueStorage = gameObject.GetComponent<ValueStorage>();
        im = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
    }

    void Start()
    {
        audioController = GameObject.Find("PanelKeys").GetComponent<AudioController>();
        var parent = transform;
        while (!parent.CompareTag("Channel"))
        {
            parent = parent.parent;
        }
        channel = parent.name;
        SlideFader(0, initialMove: true); // Initial "move" to get initial value from position
    }

    // Update is called once per frame
    void Update () 
    {
        if (isClicked && (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0))
        {
            SlideFader(Input.mouseScrollDelta.y / 2);
        }
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

    private void SlideFader(float inputForce, bool initialMove = false)
    {
        // Slide Fader only when it's the current target object or not needed for future interactions
        if (im.GetCurrentInteraction().TargetObject == gameObject 
            || !blockedChannels.Contains(channel) 
            || initialMove
            || im.FinalMixingIsActive())
        {
            verticalMovement = inputForce * sensitivityY * Time.deltaTime;
            float posX = transform.localPosition.x - verticalMovement;
            float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
            transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
            value = GetNonLinearFaderValue(faderPvr);
            ChangeValueText();
            valueStorage.SetValue(value, gameObject);
            audioController.SetFaderVolume(transform.name, channel, value);
        }
        else
        {
            ChangeValueText();
        }
    }

    private void ChangeValueText()
    {
        if (value == -80)
            canvasValueText.text = "-" + "\u221E" + " dB";
        else
            canvasValueText.text = value.ToString("F2") + " dB";
    }

    private void UpdateSound()
    {
        

    }

    void OnMouseEnter()
    {

    }

    void OnMouseExit()
    {

    }

}