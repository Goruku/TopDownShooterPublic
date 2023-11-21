using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PolygonCollider2D))]
public class SmartFOV : VariableRenderObserver
{
    public CircleCollider2D circleCollider2D;
    public LayerMask layerMask;
    public ContactFilter2D cornerCastFilter;
    public float cornerCastSimilitude = 0.005f;
    public MeshFilter meshFilter;
    public PolygonCollider2D polygonCollider2D;
    public Transform anchor;

    public bool fanCast = false;
    public bool cornerCast = true;
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
        if (cornerCast)
        {
            List<Vector3> vertices = new List<Vector3>();
            FindCornerVertices(vertices);
            vertices = vertices.OrderBy(vertexPosition =>
                Quaternion.LookRotation(Vector3.forward, (Vector3) vertexPosition - currentPosition).eulerAngles[2]).ToList();
            var polygon = new Vector2[vertices.Count*2 + 1];
            for (int i = 0; i < vertices.Count ; i++)
            {
                CornerCast(vertices, polygon, i);
            }
            polygonCollider2D.points = contactPoints;
            meshFilter.mesh = polygonCollider2D.CreateMesh(false, false);
        }
        if (!fanCast && !cornerCast)
        {
            ClearPolygonCollider();
        }
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
            if (!rayHit.collider)
            {
                contactPoints[i] = currentPosition + direction.normalized * circleCollider2D.radius;
            }
            else contactPoints[i] = rayHit.point;
                
        }
        contactPoints[contactPoints.Length - 1] = anchor.position;
    }

    private void FindCornerVertices(List<Vector3> vertices)
    {
        Collider2D[] colliders = new Collider2D[50];
        circleCollider2D.OverlapCollider(cornerCastFilter, colliders);
        foreach (var collider in this.colliders)
        {
            if (collider)
            {
                vertices.AddRange(collider.CreateMesh(true, true).vertices);
            }
        }
    }

    private void WindVertices(out List<Vector2> woundVertices, in List<Vector2> vertices)
    {
        var currentPosition = anchor.position;

        woundVertices = new List<Vector2>();
    } 

    private void CornerCast(in List<Vector3> vertices, in Vector2[] polygon, int index)
    {
        var maxEndpoint = (Vector2) (vertices[index] - anchor.position).normalized * circleCollider2D.radius;
        var contacts = new RaycastHit2D[2];
        Physics2D.Linecast(anchor.position, maxEndpoint, cornerCastFilter, contacts);
        if (!contacts[0].collider) {
            polygon[2*index] = maxEndpoint;
            polygon[2*index + 1] = maxEndpoint;
            return;
        }
        polygon[index] = contacts[0].point;
        var nextContactPoint = contacts[1].collider ? contacts[1].point : maxEndpoint;
        var backwardsCast = Physics2D.Linecast(nextContactPoint, contacts[0].point, cornerCastFilter.layerMask);
        if ((backwardsCast.point - contacts[0].point).magnitude <= cornerCastSimilitude)
            polygon[2*index + 1] = nextContactPoint;
        else
            polygon[2*index + 1] = contacts[0].point;
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

    protected override void FlagObjectsForRender()
    {
        colliders = new Collider2D[maxColliderCount];
        polygonCollider2D.OverlapCollider(viewPointRenderContactFilter, colliders);
        foreach (var collider in colliders)
        {
            if (!collider) break;
            var variableRender = collider.GetComponent<VariableRender>();
            if (variableRender)
                variableRender.seenBy |= attachedPlayer;
        }
    }
    
}
