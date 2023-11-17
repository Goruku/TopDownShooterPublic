using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Collider2D[] colliders = new Collider2D[25];
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

        HashSet<Vector2> vertexSet = new HashSet<Vector2>();
        
        vertices = vertices.OrderBy(vertexPosition =>
        {
            vertexSet.Add(vertexPosition);
            return Quaternion.LookRotation(Vector3.forward, vertexPosition - currentPosition).eulerAngles[2];
        }).ToList();
        
        foreach (var vertex in vertices)
        {
            var linecastHit = Physics2D.Linecast(currentPosition, vertex, layerMask);

            if (vertexSet.Contains(linecastHit.point))
            {
                var distanceVector = vertex - currentPosition;
                var raycastHit = Physics2D.Raycast(vertex, distanceVector,
                    circleCollider2D.radius - distanceVector.magnitude, layerMask);
                meshPositions.Add(raycastHit.point);
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
