using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform pivot;
    public Transform target;
    public float leftBound = 240f;
    public float rightBound = 120f;
    public float speed = 1f;
    private bool clockwise = true;

    private void Awake()
    {
        transform.LookAt(target);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetDir = target.position - pivot.position;
        Quaternion rotation = Quaternion.LookRotation(targetDir);

        pivot.rotation = Quaternion.Slerp(pivot.rotation, rotation, step);

        if (clockwise)
        {
            pivot.RotateAround(target.position, Vector3.up, step);
            if (pivot.eulerAngles.y >= leftBound)
            {
                clockwise = false;
            }
        }
        else
        {
            pivot.RotateAround(target.position, Vector3.down, step);
            if (pivot.eulerAngles.y <= rightBound)
            {
                clockwise = true;
            }
        }
    }
}
