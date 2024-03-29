using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMover : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public PlayerInput playerInput;

    public bool isMoving;
    public bool shouldStop;
    public Vector2 velocityVector;
    public float movementSpeed;

    private void OnEnable()
    {
        playerInput.actions["Move"].started += StartMovement;
        playerInput.actions["Move"].performed += StartMovement;
        playerInput.actions["Move"].canceled += EndMovement;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].started -= StartMovement;
        playerInput.actions["Move"].performed -= StartMovement;
        playerInput.actions["Move"].canceled -= EndMovement;
    }

    private void StartMovement(InputAction.CallbackContext callbackContext)
    {
        velocityVector = callbackContext.ReadValue<Vector2>().normalized * movementSpeed;
        isMoving = true;
    }

    private void EndMovement(InputAction.CallbackContext callbackContext)
    {
        velocityVector = new Vector2();
        isMoving = false;
        shouldStop = true;
    }

    private void FixedUpdate()
    {
        if (!isMoving && !shouldStop) return;
        if (rigidBody2D.velocity.magnitude > movementSpeed) return;
        rigidBody2D.velocity = velocityVector.magnitude != 0 ? velocityVector : new Vector2();
        shouldStop = false;
    }
}
