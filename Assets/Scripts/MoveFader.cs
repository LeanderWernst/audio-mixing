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

        if (Input.GetMouseButton(0) &&  transform.localPosition.z >= -10f && movedownY > 0)
        {
            transform.Translate(Vector3.back * movedownY);
        }
        else if (Input.GetMouseButton(0) && transform.localPosition.z <= 1.3f && movedownY < 0)
        {
            transform.Translate(Vector3.back * movedownY);
        }
        movedownY = 0.0f;
        audioController.SetChannelLevel("MasterVol", transform.localPosition.z);
    }

    private void UpdateSound()
    {
        

    }

}