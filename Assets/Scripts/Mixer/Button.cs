using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool isOn = false;
    private ValueStorage valueStorage;
    public bool isClicked = false;
    private bool isMoving = false;
    private bool hasLED = false;
    [SerializeField] private string LEDTag = "Light";

    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private float smoothTime = 0.1F;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    TextMeshProUGUI canvasValueText;

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        valueStorage = gameObject.GetComponent<ValueStorage>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        startPosition = transform.position;
        endPosition = transform.TransformPoint(new Vector3(0, 0, -0.00057567f));
        hasLED = transform.parent != null && transform.parent.tag == LEDTag;
        if (hasLED)
            transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    private void Update()
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
        else if (hasLED && !isOn)
            transform.parent.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        canvasValueText.text = isOn ? "on" : "off";
        valueStorage.SetValue(isOn ? 1f : 0f, gameObject);
    }
}
