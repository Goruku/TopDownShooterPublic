using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAimer : MonoBehaviour
{
    
    public InputActionAsset inputs;

    public CinemachineVirtualCamera virtualCamera;
    public Transform aimTarget;
    public Transform regularTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.actionMaps[0]["Aim"].IsPressed())
        {
            virtualCamera.Follow = aimTarget;
            virtualCamera.LookAt = aimTarget;
        }
        else
        {
            virtualCamera.Follow = regularTarget;
            virtualCamera.LookAt = regularTarget;
        }
    }
}
