using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerFactory : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public Camera activeCamera;
    public GameObject playerGizmos;

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += CreatePlayerGizmo;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= CreatePlayerGizmo;
    }

    void CreatePlayerGizmo(PlayerInput playerInput)
    {
        var aimingGizmos = Instantiate(playerGizmos).GetComponent<AimingGizmos>();
        var physicsTargeter = playerInput.GetComponent<PhysicsTargeter>();
        physicsTargeter.target = aimingGizmos.cursor.transform;
        aimingGizmos.cursor.playerInput = playerInput;
        aimingGizmos.cursor.activeCamera = activeCamera;
        aimingGizmos.physicsTargeter = physicsTargeter;
        aimingGizmos.gameObject.SetActive(true);
        Debug.Log(playerInput.currentControlScheme);
    }
}

public delegate void CameraChangeEvent(Camera newCamera);