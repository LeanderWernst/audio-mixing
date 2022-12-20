using UnityEngine;
using System.Collections;
using TMPro;

public class MoveFader : MonoBehaviour
{

    [SerializeField] private float upperPosBoundary = 0.0236f;
    [SerializeField] private float lowerPosBoundary = 0.06782f;
    [SerializeField] private float sensitivityY = 0.05f;
    
    private PositionValueRelation[] faderPvr = FaderPvr.relation;
    public float value;
    private float verticalMovement;
    public bool isClicked = false;
    public AudioController audioController;
    TextMeshProUGUI canvasValueText;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () 
    {
        if (isClicked)
        {
            SlideFader(Input.mouseScrollDelta.y * 2);
        }
    }

    private void OnMouseDrag()
    {
        SlideFader(Input.GetAxis("Mouse Y"));
    }

    private void OnMouseDown()
    {

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

    private void SlideFader(float inputForce)
    {
        verticalMovement = inputForce * sensitivityY * Time.deltaTime;
        float posX = transform.localPosition.x - verticalMovement;
        float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
        transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
        value = GetNonLinearFaderValue(faderPvr);
        ChangeValueText();
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