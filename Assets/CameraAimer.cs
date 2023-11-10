using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAimer : MonoBehaviour
{
    public PlayerInput playerInput;

    public CinemachineVirtualCamera virtualCamera;
    public Transform aimTarget;
    public Transform regularTarget;

    private void OnEnable()
    {
        playerInput.actions["Aim"].started += StartAim;
        playerInput.actions["Aim"].canceled += EndAim;
    }

    private void OnDisable()
    {
        playerInput.actions["Aim"].started -= StartAim;
        playerInput.actions["Aim"].canceled -= EndAim;
    }

    void StartAim(InputAction.CallbackContext callbackContext)
    {
        virtualCamera.Follow = aimTarget;
        virtualCamera.LookAt = aimTarget;
    }

    void EndAim(InputAction.CallbackContext callbackContext)
    {
        virtualCamera.Follow = regularTarget;
        virtualCamera.LookAt = regularTarget;
    }
}
