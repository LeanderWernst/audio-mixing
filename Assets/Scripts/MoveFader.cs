using UnityEngine;
using System.Collections;

public class MoveFader : MonoBehaviour
{
    public float movedownY = 0.0f;

    public float sensitivityY = 1f;

    public AudioController instrument;

    // Use this for initialization
    void Start()
    {
        instrument = GetComponent<AudioController>();
    }

// Update is called once per frame
void Update () {


}

    private void OnMouseDrag()
    {
        movedownY += Input.GetAxis("Mouse Y") * sensitivityY;

        if (Input.GetMouseButton(0))
        {

            transform.Translate(Vector3.back * movedownY);

        }
        Debug.Log(transform.position.z);
        movedownY = 0.0f;
        
    }

    private void UpdateSound()
    {
        //instrument.SetLevel()

    }

}