using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera Cinecamera;
    public float cameraAdjustment;

    // Start is called before the first frame update
    void Start()
    {
        Cinecamera = gameObject.GetComponent<CinemachineVirtualCamera>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y += cameraAdjustment;
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y -= cameraAdjustment;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y -= cameraAdjustment;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y += cameraAdjustment;
        }
    }
}
