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

    void OnMouseDrag()
	{
        angle += Input.GetAxis("Mouse Y") * rotationSpeed;
        angle = Mathf.Clamp(angle, minRotation, maxRotation);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
    }
}