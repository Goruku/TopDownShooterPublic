using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += DebugLogControlSchemeName;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= DebugLogControlSchemeName;
    }

    void DebugLogControlSchemeName(PlayerInput playerInput)
    {
        Debug.Log(playerInput.currentControlScheme);
    }
}

public delegate void CameraChangeEvent(Camera newCamera);