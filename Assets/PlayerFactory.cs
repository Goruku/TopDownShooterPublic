using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    public int playerCount = 0;
    public PlayerInputManager playerInputManager;

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += DebugLogControlSchemeName;
        playerInputManager.onPlayerJoined += AssignNewVariableTargets;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= DebugLogControlSchemeName;
        playerInputManager.onPlayerJoined -= AssignNewVariableTargets;
    }

    void DebugLogControlSchemeName(PlayerInput playerInput)
    {
        Debug.Log(playerInput.currentControlScheme);
    }

    void AssignNewVariableTargets(PlayerInput playerInput)
    {
        var variableRenderBrain = playerInput.GetComponent<VariableRenderBrain>();

        variableRenderBrain.SetShowTo((uint) 1 << playerCount);
        variableRenderBrain.SetShowOnly(true);
        variableRenderBrain.SetCameraPlayer((uint) 1 << playerCount);
        variableRenderBrain.SetObserverPlayer((uint) 1 << playerCount);
        variableRenderBrain.SetName("Player" + (1 << playerCount));
        playerCount++;
    }
}

public delegate void CameraChangeEvent(Camera newCamera);