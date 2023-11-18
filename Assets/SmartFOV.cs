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

    public bool addOutsideVertex = false;
    public bool addSimilarVertex = true;
    public bool addLineIntersection = true;
    public bool addRaycastLimit = true;
    public bool addRaycast = true;
    public int raycastChosen = 1;
    public int raycastDepth = 2;

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
        Collider2D[] colliders = new Collider2D[50];
        circleCollider2D.OverlapCollider(new ContactFilter2D(), colliders);
        var currentPosition = anchor.position;
        meshPositions.Clear();
        vertices.Clear();
        meshPositions.Add(currentPosition);
        
        foreach (var collider in colliders)
        {
            if (!collider) break;
            if (!collider.CompareTag("VisBlocker")) continue;
            
            List<Vector3> vertices = new List<Vector3>();
            collider.CreateMesh(false, false).GetVertices(vertices);
            
            this.vertices.AddRange(vertices);
        }
        vertices = vertices.OrderBy(vertexPosition => Quaternion.LookRotation(Vector3.forward, vertexPosition - currentPosition).eulerAngles[2]).ToList();
        
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
            if (((Vector2) vertex - linecastHit.point).magnitude <= similitude)
            {
                if (addSimilarVertex)
                    meshPositions.Add(vertex);
                var raycastHits = new RaycastHit2D[raycastDepth];
                Physics2D.RaycastNonAlloc(vertex + (distanceVector.normalized*pierce),
                    2*distanceVector, raycastHits, circleCollider2D.radius, layerMask);
                
                Debug.DrawRay(vertex, distanceVector, Color.red);
                
                if (((Vector2) currentPosition - raycastHits[raycastChosen].point).magnitude >= circleCollider2D.radius)
                {
                    if (addRaycastLimit)
                        meshPositions.Add(circleCollider2D.radius * distanceVector.normalized);
                }
                else
                {
                    if (addRaycast)
                    {
                        meshPositions.Add(raycastHits[raycastChosen].point);
                    }
                }
            }
            else
            {
                if (addLineIntersection)
                    meshPositions.Add(linecastHit.point);
            }
        }

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
