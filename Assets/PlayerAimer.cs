using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAimer : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Transform target;

    public CinemachineVirtualCamera virtualCamera;
    public InputActionAsset inputs;

    public Transform distanceTracker;
    private SpriteRenderer _trackerRenderer;
    public Transform trueTarget;
    public LineRenderer trueTargetLineRenderer;
    
    public float rotationMultiplier = 1;
    public AnimationCurve rotationCurve;
    public float lookDifferential;
    
    public float minRange;
    public float maxRange;
    public float distanceMultiplier = 1;
    public AnimationCurve distanceCurve;
    public float distanceDifferential;
    
    public bool counterClockwise;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _trackerRenderer = distanceTracker.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!target) return;
        var targetingVector = target.position - rigidBody2D.transform.position;
        var currentRotation = rigidBody2D.transform.rotation;
        
        var lookRotation = Quaternion.LookRotation(Vector3.forward, targetingVector);
        //https://stackoverflow.com/questions/25498263/determining-if-quarternion-rotation-is-clockwise-or-counter-clockwise
        counterClockwise = Vector3.Dot(Vector3.Cross(currentRotation*Vector3.up, targetingVector), Vector3.forward) <= 0;
        
        //https://math.stackexchange.com/questions/90081/quaternion-distance
        var innerProduct = Quaternion.Dot(lookRotation, currentRotation);
        lookDifferential = 1 - innerProduct*innerProduct;
        var rotationSpeed = rotationCurve.Evaluate(lookDifferential) * rotationMultiplier;

        var targetingVectorMagnitude = targetingVector.magnitude;
        distanceDifferential = Mathf.Clamp(targetingVectorMagnitude, minRange, maxRange)/maxRange;
        rotationSpeed *= distanceCurve.Evaluate(distanceDifferential) * distanceMultiplier;
        //TODO: angular drag with distance curve?

        rigidBody2D.angularVelocity += counterClockwise ? -rotationSpeed : rotationSpeed;

        distanceTracker.position = (Vector3) rigidBody2D.position + maxRange*distanceDifferential*targetingVector.normalized;
        
        if (distanceDifferential < 1 && targetingVectorMagnitude > minRange)
            _trackerRenderer.enabled = false;
        else
            _trackerRenderer.enabled = true;

        if (inputs.actionMaps[0]["Aim"].IsPressed())
        {
            virtualCamera.Follow = distanceTracker;
            virtualCamera.LookAt = distanceTracker;
        }
        else
        {
            virtualCamera.Follow = rigidBody2D.transform;
            virtualCamera.LookAt = rigidBody2D.transform;
        }

        
        if (trueTarget)
        {
            trueTarget.position = rigidBody2D.transform.position + currentRotation * (Vector3.up * (maxRange * distanceDifferential));
            if (trueTargetLineRenderer)
            {
                trueTargetLineRenderer.SetPositions(new Vector3[]{rigidBody2D.position, trueTarget.position});
            }
        }
    }
}
