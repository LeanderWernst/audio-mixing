using UnityEngine;
using System.Collections;
public class MoveFader : MonoBehaviour
{
    private float movedownY = 0.0f;

    public float sensitivityY = 1f;

    public AudioController audioController;

    // Use this for initialization
    void Start()
    {
       // audioController = GetComponent<AudioController>();
    }

// Update is called once per frame
void Update () {


}

    private void OnMouseDrag()
    {
        movedownY += Input.GetAxis("Mouse Y") * sensitivityY;

        if (Input.GetMouseButton(0) &&  transform.localPosition.x >= -0.057f  && movedownY > 0)
        {
            transform.Translate(Vector3.up * movedownY);
        }
        else if (Input.GetMouseButton(0) && transform.localPosition.x <= -0.012f && movedownY < 0)
        {
            transform.Translate(Vector3.up * movedownY);
        }
        movedownY = 0.0f;
        audioController.SetChannelLevel("MasterVol", transform.localPosition.z);
    }

    private void UpdateSound()
    {
        

    }

}