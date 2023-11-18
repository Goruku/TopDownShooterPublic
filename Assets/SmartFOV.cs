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
            
            if (linecastHit.distance <= 0)
            {
                meshPositions.Add(circleCollider2D.radius * distanceVector.normalized);
            } 
            
            //arbitrary low distance "close enough to be the same" also power of two
            else if (((Vector2) vertex - linecastHit.point).magnitude <= similitude)
            {
                meshPositions.Add(vertex);

                var raycastHits = new RaycastHit2D[2];
                Physics2D.RaycastNonAlloc(vertex +distanceVector.normalized*pierce, distanceVector, raycastHits,
                    float.MaxValue, layerMask);
                if (raycastHits[1].IsUnityNull()) continue;
                if (raycastHits[1].distance <= 0)
                {
                    meshPositions.Add(circleCollider2D.radius * distanceVector.normalized);
                }
                else
                {
                    //meshPositions.Add(raycastHits[1].point);
                }
            }
            else
            {
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
