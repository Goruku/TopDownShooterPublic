using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(MeshFilter))]
public class SmartFOV : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public LayerMask layerMask;
    public MeshFilter meshFilter;
    public Transform anchor;

    public bool extractNewVertices = true;
    public bool windChunkVertices = true;

    public bool addOutsideVertex = false;
    public bool addSimilarVertex = true;
    public bool addLineIntersection = true;
    public bool addRaycastLimit = true;
    public bool addRaycast = true;
    public int raycastChosen = 1;
    public int raycastDepth = 2;

    public int fanCount = 5;
    public float angle = 90;

    public float normalCullThreshold = -0.98f;

    public float similitude = 0.0009765625f;
    public float pierce = 0;
    public bool circles = true;
    public List<Vector3> vertices = new ();
    public List<Vector3> meshPositions = new ();

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void FixedUpdate()
    {
        meshFilter.mesh = null;
        Vector3 currentPosition = anchor.position;
        if (extractNewVertices)
            ExtractNewVertices();
        if (windChunkVertices)
            WindChunkVertices(currentPosition);
        FanCast(currentPosition);
        BuildMesh();
    }

    private void FanCast(Vector3 currentPosition)
    {
        meshPositions.Add(currentPosition);
        var startAngle = -angle / 2;
        var angleInterval = angle / fanCount;
        for (int i = 0; i < fanCount; i++)
        {
            var rayHit = Physics2D.Raycast(currentPosition,
                Quaternion.AngleAxis(startAngle + angleInterval * i, Vector3.forward) * Vector3.up,
                circleCollider2D.radius, layerMask);
            if (rayHit.distance >= 0)
                meshPositions.Add(rayHit.point);
        }
    }

    private void NewCast(Vector3 currentPosition)
    {
        meshPositions.Add(currentPosition);
        foreach (var vertex in vertices)
        {
            var distanceVector = vertex - currentPosition;
            var raycastHits = new RaycastHit2D[raycastDepth];
            Physics2D.RaycastNonAlloc(currentPosition, distanceVector, raycastHits, circleCollider2D.radius, layerMask);
            if (addOutsideVertex && raycastHits[0].distance <= 0)
            {
                meshPositions.Add(distanceVector.normalized * circleCollider2D.radius);
            }
            if (addSimilarVertex)
                meshPositions.Add(raycastHits[0].point);
            foreach (var hit in raycastHits)
            {
                if (!addRaycast) break;
                var linecast = Physics2D.Linecast(hit.point, vertex);
                if ((linecast.point - (Vector2)currentPosition).magnitude <= similitude)
                {
                    continue;
                }
                meshPositions.Add(linecast.point);
                break;
            }
        }
    }

    private void OldCast(Vector3 currentPosition)
    {
        meshPositions.Add(currentPosition);
        foreach (var vertex in vertices)
        {
            var linecastHit = Physics2D.Linecast(currentPosition, vertex, layerMask);
            var distanceVector = vertex - currentPosition;

            if (linecastHit.distance >= circleCollider2D.radius)
            {
                if (addOutsideVertex)
                    meshPositions.Add(vertex);
                continue;
            }

            //arbitrary low distance "close enough to be the same" also power of two
            if (((Vector2)vertex - linecastHit.point).magnitude <= similitude)
            {
                if (addRaycast)
                {
                    var raycastHits = new RaycastHit2D[raycastDepth];
                    Physics2D.RaycastNonAlloc(vertex + (distanceVector.normalized * pierce),
                        2 * distanceVector, raycastHits, circleCollider2D.radius, layerMask);
                    
                    linecastHit = Physics2D.Linecast(raycastHits[raycastChosen].point, vertex, layerMask);
                    Debug.DrawLine(raycastHits[raycastChosen].point, vertex);
                    if (((Vector2)vertex - linecastHit.point).magnitude <= similitude)
                        meshPositions.Add(raycastHits[raycastChosen].point);
                }
                if (addSimilarVertex)
                    meshPositions.Add(vertex);
            }
            else if (addLineIntersection && linecastHit.distance > 0)
            {
                meshPositions.Add(linecastHit.point);
            }
        }
    }

    private void ExtractNewVertices()
    {
        Collider2D[] colliders = new Collider2D[50];
        circleCollider2D.OverlapCollider(new ContactFilter2D(), colliders);
        meshPositions.Clear();
        vertices.Clear();

        foreach (var collider in colliders)
        {
            if (!collider) break;
            if (!collider.CompareTag("VisBlocker")) continue;

            List<Vector3> vertices = new List<Vector3>();
            collider.CreateMesh(false, false).GetVertices(vertices);

            this.vertices.AddRange(vertices);
        }
    }

    private void WindChunkVertices(Vector3 currentPosition)
    {
        vertices = vertices.OrderBy(vertexPosition =>
            Quaternion.LookRotation(Vector3.forward, vertexPosition - currentPosition).eulerAngles[2]).ToList();
    }

    private void BuildMesh()
    {
        var triCount = meshPositions.Count - 1;
        
        if (triCount <= 0)
        {
            return;
        }
        meshPositions.Add(meshPositions[1]);

        int[] tri = new int[3 * triCount];
        if (circles)
        {
            tri[3 * triCount - 3] = 0;
            tri[3 * triCount - 2] = meshPositions.Count - 1;
            tri[3 * triCount - 1] = 1;
        }
        for (int i = 0; i < triCount; i++)
        {
            tri[3*i] = 0;
            tri[3*i + 1] = i + 2;
            tri[3*i + 2] = i + 1;
        }
        
        meshFilter.mesh.vertices = meshPositions.ToArray();
        meshFilter.mesh.triangles = tri;
    }
}
