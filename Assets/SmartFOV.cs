using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PolygonCollider2D))]
public class SmartFOV : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public LayerMask layerMask;
    public MeshFilter meshFilter;
    public PolygonCollider2D polygonCollider2D;
    public Transform anchor;

    public uint attachedPlayer;

    public bool fanCast = true;
    public Vector2[] contactPoints;

    public int fanCount = 120;
    public float angle = 90;
    public float differentialDeadzone = 1;
    public Quaternion lastAngle;
    public Vector3 lastPosition;
    public AnimationCurve rayConcentration = new();

    public ContactFilter2D viewPointRenderContactFilter = new ContactFilter2D(){useLayerMask = true, useTriggers = true};
    public Collider2D[] colliders;
    
    public int maxColliderCount = 50;
    
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = anchor.position;
        contactPoints = new Vector2[fanCount + 1];
        if (fanCast)
        {   
            var innerProduct = Quaternion.Dot(anchor.rotation, lastAngle);
            if (lastPosition != currentPosition || 1 - innerProduct * innerProduct >= differentialDeadzone)
            {
                FanCast(currentPosition);
                SetPolygonCollider();
                lastAngle = anchor.rotation;
                lastPosition = currentPosition;
            }
        }
        else
        {
            ClearPolygonCollider();
        }
    }

    private void Update()
    {
        FlagObjectsForRender();
    }

    private void FanCast(Vector3 currentPosition)
    {
        float angleInterval = 1f / fanCount;
        var angleSplit = -angle / 2;
        for (int i = 0; i < fanCount; i++)
        {
            var direction = anchor.rotation *
                            Quaternion.AngleAxis(angleSplit*rayConcentration.Evaluate(i*angleInterval), Vector3.forward) * Vector3.up;
            var rayHit = Physics2D.Raycast(currentPosition,
                direction,
                circleCollider2D.radius, layerMask);
            if (rayHit.distance >= circleCollider2D.radius - 0.001f)
            {
                contactPoints[i] = direction.normalized * circleCollider2D.radius;
            }
            else contactPoints[i] = rayHit.point;
                
        }
        contactPoints[contactPoints.Length - 1] = anchor.position;
    }

    private void SetPolygonCollider()
    {
        polygonCollider2D.points = contactPoints;
        meshFilter.mesh = polygonCollider2D.CreateMesh(false, false);
    }

    private void ClearPolygonCollider()
    {
        polygonCollider2D.points = null;
        meshFilter.mesh = null;
    }

    private void FlagObjectsForRender()
    {
        colliders = new Collider2D[maxColliderCount];
        polygonCollider2D.OverlapCollider(viewPointRenderContactFilter, colliders);
        foreach (var collider in colliders)
        {
            if (collider)
            {
                collider.GetComponent<VariableRender>().seenBy |= attachedPlayer;
            }
        }
    }
    
}
