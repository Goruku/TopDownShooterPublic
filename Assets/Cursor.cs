using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Cursor : MonoBehaviour
{
    public PlayerInput playerInput;
    public float cursorSpeed = 0.03f;

    public Camera activeCamera;

    private void OnEnable()
    {
        playerInput.actions["Look"].started += ControlCursor;
        playerInput.actions["Look"].performed += ControlCursor;
        playerInput.actions["Look"].canceled += ControlCursor;
    }

    private void OnDisable()
    {
        playerInput.actions["Look"].started -= ControlCursor;
        playerInput.actions["Look"].performed -= ControlCursor;
        playerInput.actions["Look"].canceled -= ControlCursor;
    }

    private void OnTransformParentChanged()
    {
        OnDisable();
        OnEnable();
    }

    void ControlCursor(InputAction.CallbackContext callbackContext)
    {
        if (playerInput.currentControlScheme != "Mouse&Keyboard")
        {
            var targetPos = activeCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.value.x,
                Mouse.current.position.value.y,
                activeCamera.nearClipPlane));
            transform.position = new Vector3(targetPos.x, targetPos.y, 0);
        }
        else
        {
            Debug.Log("Controller input");
            var lookVector = callbackContext.ReadValue<Vector2>();
            transform.position += (Vector3) lookVector.normalized * cursorSpeed;
        }
    }
}
