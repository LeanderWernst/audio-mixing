using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private bool isOn = false;
    public bool isClicked = false;
    private bool isMoving = false;
    private bool hasLED = false;
    [SerializeField] private string LEDTag = "Light";

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float smoothTime = 0.1F;
    [SerializeField] private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.TransformPoint(new Vector3(0, 0, -0.00057567f));
        hasLED = transform.parent != null && transform.parent.tag == LEDTag;
        if (hasLED)
            transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isOn)
                transform.position = Vector3.SmoothDamp(transform.position, endPosition, ref velocity, smoothTime);
            else
                transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, smoothTime);
        }
        if (transform.position == endPosition || transform.position == startPosition)
            isMoving = false;
    }

    private void OnMouseUp()
    {
            isOn = !isOn;
            isMoving = true;
            if (hasLED && isOn) // Turn on LED
                transform.parent.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            else
                transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

}
