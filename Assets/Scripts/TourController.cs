using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TourController : MonoBehaviour
{
    public CinemachineBrain cine;
    public CinemachineVirtualCamera vCamOverview;
    public CinemachineVirtualCamera vCam0;
    public CinemachineVirtualCamera vCam3;
    public CinemachineVirtualCamera vCam4;
    public CinemachineVirtualCamera vCam21;
    public CinemachineVirtualCamera vCam27;
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
            case "CMvcamOverview":
                vCamOverview.Priority = 1;
                vCam0.Priority = 2;
                break;
            case "CMvcam0":
                vCam0.Priority = 1;
                vCam3.Priority = 2;
                break;
            case "CMvcam3":
                vCam3.Priority = 1;
                vCam4.Priority = 2;
                break;
            case "CMvcam4":
                vCam4.Priority = 1;
                vCam21.Priority = 2;
                break;
            case "CMvcam21":
                vCam21.Priority = 1;
                vCam27.Priority = 2;
                break;
            case "CMvcam27":
                vCam27.Priority = 1;
                vCamOverview.Priority = 2;
                break;
            default:
                // do default
                break;
        }

    }
}
