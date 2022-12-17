using UnityEngine;
using System.Collections;
public class MoveFader : MonoBehaviour
{

    [SerializeField] private float upperPosBoundary = -0.0570576f;
    [SerializeField] private float lowerPosBoundary = -0.01277522f;
    [SerializeField] private float sensitivityY = 0.05f;
    private PositionValueRelation[] pvr = new PositionValueRelation[] 
        {
            new PositionValueRelation(new float[]{-0.0570576f, -0.02687608f}, new float[]{10f, -15f}),
            new PositionValueRelation(new float[]{-0.02687608f, -0.0224244f}, new float[]{-15f, -20f}),
            new PositionValueRelation(new float[]{-0.0224244f, -0.018016f}, new float[]{-20f, -30f}),
            new PositionValueRelation(new float[]{-0.018016f, -0.01534911f}, new float[]{-30f, -40f}),
            new PositionValueRelation(new float[]{-0.01534911f, -0.01277522f}, new float[]{-40f, -1000f})
        };

    private float verticalMovement;
       
    public bool isClicked = false;

    public AudioController audioController;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update () 
    {
        if (isClicked)
        { 
            verticalMovement = Input.mouseScrollDelta.y * sensitivityY*2 * Time.deltaTime;
            float posX = transform.localPosition.x - verticalMovement;
            float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
            transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
        }

    }

    private void OnMouseDrag()
    {
        verticalMovement = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        float posX = transform.localPosition.x - verticalMovement;
        float clampedPosX = Mathf.Clamp(posX, upperPosBoundary, lowerPosBoundary);
        transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
        Debug.Log(GetNonLinearFaderValue(pvr));
    }

    float GetNonLinearFaderValue(PositionValueRelation[] pvr)
    {
        var pos = transform.localPosition.x;
        foreach (var relation in pvr)
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