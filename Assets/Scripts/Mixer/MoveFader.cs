using UnityEngine;
using System.Collections;
public class MoveFader : MonoBehaviour
{


    [SerializeField] private float sensitivityY = 0.05f;
    private float movedownY = 0.0f;

    private Vector3 startPos;
    private Vector3 endPos;

    public bool isClicked = false;

    public AudioController audioController;

    /*private Renderer rend;
    private Color startCol;*/

    // Use this for initialization
    void Start()
    {
        startPos = transform.localPosition;
        endPos = startPos + new Vector3(-0.045f, 0.0f, 0.0f);

        /*rend = GetComponent<Renderer>();
        startCol = rend.material.color;*/
        // audioController = GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update () 
    {


    }

    private void OnMouseDrag()
    {
        var pos = transform.localPosition;
        
        movedownY += Input.GetAxis("Mouse Y") * sensitivityY;

        if (Input.GetMouseButton(0) 
            && (pos.x >= endPos.x && pos.x <= startPos.x) 
            && (movedownY != 0))
        {
            transform.Translate(Vector3.up * movedownY);
        }
        if (pos.x < endPos.x)           // Ensure position does not go over bounds
        { 
            transform.localPosition = endPos;
        }
        else if (pos.x > startPos.x)    // Ensure position does not go over bounds
        { 
            transform.localPosition = startPos;
        }
        movedownY = 0.0f;

        /*if (Input.GetMouseButton(0) && pos.x >= -0.057f && movedownY > 0)
        {
            transform.Translate(Vector3.up * movedownY);
            if (pos.x < -0.057f) // Ensure position does not go over bounds
                transform.localPosition.Set(-0.057f, pos.y, pos.z);
        }
        else if (Input.GetMouseButton(0) && pos.x <= -0.012f && movedownY < 0)
        {
            transform.Translate(Vector3.up * movedownY);
            if (pos.x > -0.012f) // Ensure position does not go over bounds
                transform.localPosition.Set(-0.012f, pos.y, pos.z);
        }
        movedownY = 0.0f;*/
        //audioController.SetChannelLevel("MasterVol", transform.localPosition.z);
    }

    private void UpdateSound()
    {
        

    }

    void OnMouseEnter()
    {
        //rend.material.color = Color.red;
        //rend.material.EnableKeyword("_EMISSION");
        //rend.material.SetColor("_EmissionColor", Color.yellow);
    }

    void OnMouseExit()
    {
        //rend.material.color = startCol;
        //rend.material.DisableKeyword("_EMISSION");
    }

}