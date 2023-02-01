using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cvCamera;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float panSpeed = 6f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float zoomMin = 7f;
    [SerializeField] private float zoomMax = 50f;

    public float minZ = 13.2f;
    public float maxZ = 24.2f;
    public float minX = -5f;
    public float maxX = 12.42f;


    private Transform camTransform;

    // Properties for cam reset
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private float initialFov;
    private float currentFov;
    private float duration = 1f;
    private float elapsedTime = 0f;

    private CinemachineBrain cinemachineBrain;
    [SerializeField] private TourController tourController;

    private void Awake()
    {
        cvCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        camTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialFov = cvCamera.m_Lens.FieldOfView;
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        currentFov = cvCamera.m_Lens.FieldOfView;
    }

    private void LateUpdate()
    {
        if (cinemachineBrain.ActiveVirtualCamera != null && cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject == gameObject)
        {
            if (Input.GetMouseButton(1))
            {
                // Zoom in/out
                float zoom = Input.GetAxis("Mouse ScrollWheel");
                cvCamera.m_Lens.FieldOfView -= zoom * zoomSpeed;
                cvCamera.m_Lens.FieldOfView = Mathf.Clamp(cvCamera.m_Lens.FieldOfView, zoomMin, zoomMax);

                // Rotate view
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;
                transform.Rotate(-rotationY, rotationX, 0, Space.Self);
                transform.rotation = Quaternion.Euler(
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    0
                );
            
                //Debug.Log("X: " + camTransform.position.x + " Y: " + camTransform.position.y + " Z: " + camTransform.position.z);
            }

            // Zoom in (additional to scrollwheel)
            if (Input.GetKey(KeyCode.E))
            {
                cvCamera.m_Lens.FieldOfView -= zoomSpeed * Time.deltaTime;
                cvCamera.m_Lens.FieldOfView = Mathf.Clamp(cvCamera.m_Lens.FieldOfView, zoomMin, zoomMax);
            }

            // Zoom out (additional to scrollwheel)
            if (Input.GetKey(KeyCode.Q))
            {
                cvCamera.m_Lens.FieldOfView += zoomSpeed * Time.deltaTime;
                cvCamera.m_Lens.FieldOfView = Mathf.Clamp(cvCamera.m_Lens.FieldOfView, zoomMin, zoomMax);
            }

            // Pan forward/backward
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                float pan = Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;
                Vector3 pos = cvCamera.transform.position;
                pos += cvCamera.transform.forward * pan;
                pos.y = camTransform.position.y;
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
                cvCamera.transform.position = pos;
            }

            // Pan left/right
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                float pan = Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
                Vector3 pos = cvCamera.transform.position;
                pos += cvCamera.transform.right * pan;
                pos.y = camTransform.position.y;
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
                cvCamera.transform.position = pos;
            }

            // Change Camera
            if (Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift))
            {
                tourController.nextCamera();
            }

        }
        // Reset all cameras with script to initial position
        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
        {
            elapsedTime = 0f;
            currentPosition = transform.position;
            currentRotation = transform.rotation;
            currentFov = cvCamera.m_Lens.FieldOfView;
        }
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, initialPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(currentRotation, initialRotation, elapsedTime / duration);
            cvCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFov, initialFov, elapsedTime / duration);
        }

    }
}

