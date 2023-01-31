using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TourController : MonoBehaviour
{
    public CinemachineBrain cine;
    public CinemachineVirtualCamera vCam0;
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nextCamera()
    {
        switch (cine.ActiveVirtualCamera.Name)
        {
            case "CMvcam0":
                vCam0.Priority = 1;
                vCam1.Priority = 2;
                break;
            case "CMvcam1":
                vCam1.Priority = 1;
                vCam2.Priority = 2;
                break;
            case "CMvcam2":
                vCam2.Priority = 1;
                vCam0.Priority = 2;
                break;
            default:
                // do default
                break;
        }
    }


}
