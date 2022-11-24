	using UnityEngine;
 
public class MoveKnob : MonoBehaviour
{
	public bool isClicked = false;

	float rotationSpeed = 0.2f;

	void OnMouseDrag()
	{
		float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
		float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
		// select the axis by which you want to rotate the GameObject
		//transform.RotateAround(Vector3.down, XaxisRotation);
		//transform.RotateAround(Vector3.right, YaxisRotation);
		//transform.RotateAroundLocal(axis: Vector3.up, angle: YaxisRotation);

		transform.Rotate(axis: Vector3.up, angle: YaxisRotation);

		
	}
}