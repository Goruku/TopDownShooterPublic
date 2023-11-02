using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(PhysicsTargeter))]
public class AimingGizmos : MonoBehaviour
{

    [FormerlySerializedAs("playerAimer")] public  PhysicsTargeter physicsTargeter;
    
    public CinemachineVirtualCamera virtualCamera;
    
    public Transform trueTarget;
    public LineRenderer trueTargetLineRenderer;
    
    public Transform distanceTracker;
    private SpriteRenderer _trackerRenderer;

    private void Awake()
    {
        physicsTargeter = GetComponent<PhysicsTargeter>();
        _trackerRenderer = distanceTracker.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var targetingVector = physicsTargeter.GetTargetingVector().normalized;
        
        distanceTracker.position = (Vector3) physicsTargeter.rigidBody2D.position + physicsTargeter.maxRange*
            physicsTargeter.distanceDifferential*targetingVector.normalized;
        
        if (physicsTargeter.distanceDifferential < 1 && targetingVector.magnitude > physicsTargeter.minRange)
            _trackerRenderer.enabled = false;
        else
            _trackerRenderer.enabled = true;
        
        if (trueTarget)
        {
            trueTarget.position = physicsTargeter.rigidBody2D.transform.position + physicsTargeter.GetCurrentRotation()
                * (Vector3.up * (physicsTargeter.maxRange * physicsTargeter.distanceDifferential));
            if (trueTargetLineRenderer)
            {
                trueTargetLineRenderer.SetPositions(new Vector3[]{physicsTargeter.rigidBody2D.position, trueTarget.position});
            }
        }
    }
}
