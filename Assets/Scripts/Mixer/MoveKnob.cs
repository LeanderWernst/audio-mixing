	using UnityEngine;
 
public class MoveKnob : MonoBehaviour
{
	public bool isClicked = false;

	[SerializeField] private float minRotation = -105.0f;
	[SerializeField] private float maxRotation = 185.0f;
	[SerializeField] private float rotationSpeed = 4.0f;

	private float angle;

    private void Start()
    {
        angle = transform.localEulerAngles.z;        
    }

    private void Update()
    {
        if (isClicked) 
        {
            angle += Input.mouseScrollDelta.y * rotationSpeed;
            angle = Mathf.Clamp(angle, minRotation, maxRotation);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        }
    }

    void OnMouseDrag()
	{
        angle += Input.GetAxis("Mouse Y") * rotationSpeed;
        angle = Mathf.Clamp(angle, minRotation, maxRotation);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        Debug.Log("Knobvalue from 0-100: " + GetKnobValue(0, 100));
    }

    // Returns calculated knob value base on given min-max-scale
    // Get values between 0-100: min = 0, max = 100
    float GetKnobValue(float min, float max)
    {
        var angle = transform.localEulerAngles.z;
        angle = angle > maxRotation ? angle - 360 : angle;
        if (minRotation != maxRotation)
            return (angle - minRotation) * (max - min) / (maxRotation - minRotation) + min;
        else
            return 0;
    }
}