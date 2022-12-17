using UnityEngine;
using System.Collections;
public class MoveFader : MonoBehaviour
{

    [SerializeField] private float uppperPosBoundary = -0.05798f;
    [SerializeField] private float lowerPosBoundary = -0.01298f;
    [SerializeField] private float sensitivityY = 0.05f;
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
            float clampedPosX = Mathf.Clamp(posX, uppperPosBoundary, lowerPosBoundary);
            transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
        }

    }

    private void OnMouseDrag()
    {
        verticalMovement = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        float posX = transform.localPosition.x - verticalMovement;
        float clampedPosX = Mathf.Clamp(posX, uppperPosBoundary, lowerPosBoundary);
        transform.localPosition = new Vector3(clampedPosX, transform.localPosition.y, transform.localPosition.z);
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