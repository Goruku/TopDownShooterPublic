using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsTargeter : MonoBehaviour
{
    public Transform target;
    
    public float rotationMultiplier = 1;
    public AnimationCurve rotationCurve;
    public float lookDifferential;
    
    public float minRange;
    public float maxRange;
    public float distanceMultiplier = 1;
    public AnimationCurve distanceCurve;
    public float distanceDifferential;
    
    public bool counterClockwise;

    public delegate void PhysicsTargeterUpdate();

    public PhysicsTargeterUpdate afterFixedUpdate = () => {};
    
    private Rigidbody2D _rigidBody2D;
    private Vector3 _targetingVector;
    private Quaternion _currentRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!target) return;
        _targetingVector = target.position - transform.position;
        _currentRotation = transform.rotation;
        
        var lookRotation = Quaternion.LookRotation(Vector3.forward, _targetingVector);
        //https://stackoverflow.com/questions/25498263/determining-if-quarternion-rotation-is-clockwise-or-counter-clockwise
        counterClockwise = Vector3.Dot(Vector3.Cross(_currentRotation*Vector3.up, _targetingVector), Vector3.forward) <= 0;
        
        //https://math.stackexchange.com/questions/90081/quaternion-distance
        var innerProduct = Quaternion.Dot(lookRotation, _currentRotation);
        lookDifferential = 1 - innerProduct*innerProduct;
        var rotationSpeed = rotationCurve.Evaluate(lookDifferential) * rotationMultiplier;

        var targetingVectorMagnitude = _targetingVector.magnitude;
        distanceDifferential = Mathf.Clamp(targetingVectorMagnitude, minRange, maxRange)/maxRange;
        rotationSpeed *= distanceCurve.Evaluate(distanceDifferential) * distanceMultiplier;
        //TODO: angular drag with distance curve?

        _rigidBody2D.angularVelocity += counterClockwise ? -rotationSpeed : rotationSpeed;

        afterFixedUpdate();
    }

    public Vector3 GetTargetingVector()
    {
        return _targetingVector;
    }

    public Quaternion GetCurrentRotation()
    {
        return _currentRotation;
    }
}
