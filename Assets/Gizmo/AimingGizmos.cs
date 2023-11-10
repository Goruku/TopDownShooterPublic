using System;
using Cinemachine;
using UnityEngine;

public class AimingGizmos : MonoBehaviour
{

    public PhysicsTargeter physicsTargeter;
    
    public Transform trueTarget;
    public LineRenderer trueTargetLineRenderer;

    public Cursor cursor;
    
    public Transform distanceTracker;
    private SpriteRenderer _trackerRenderer;

    private void Awake()
    {
        _trackerRenderer = distanceTracker.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        physicsTargeter.afterUpdate += UpdateGizmos;
    }

    private void OnDisable()
    {
        physicsTargeter.afterUpdate -= UpdateGizmos;
    }

    private void UpdateGizmos()
    {
        var targetingVector = physicsTargeter.GetTargetingVector().normalized;
        
        distanceTracker.position = (Vector3) physicsTargeter.transform.position + physicsTargeter.targetingRange.maxRange*
            physicsTargeter.distanceDifferential*targetingVector.normalized;
        
        if (physicsTargeter.distanceDifferential < 1 && targetingVector.magnitude > physicsTargeter.targetingRange.minRange)
            _trackerRenderer.enabled = false;
        else
            _trackerRenderer.enabled = true;
        
        if (trueTarget)
        {
            trueTarget.position = physicsTargeter.transform.position + physicsTargeter.GetCurrentRotation()
                * (Vector3.up * (physicsTargeter.targetingRange.maxRange * physicsTargeter.distanceDifferential));
            if (trueTargetLineRenderer)
            {
                trueTargetLineRenderer.SetPositions(new Vector3[]{physicsTargeter.transform.position, trueTarget.position});
            }
        }
    }
}
