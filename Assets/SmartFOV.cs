using System;
using System.Collections;
using System.Collections.Generic;
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
    [FormerlySerializedAs("vertex")] public List<Vector3> vertices = new ();
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
        
        foreach (var vertex in vertices)
        {
            var raycastHit2D = Physics2D.Linecast(currentPosition, vertex, layerMask);
            meshPositions.Add(raycastHit2D.point);
        }
        
        var rayCount = meshPositions.Count - 1;
        
        if (rayCount <= 0)
        {
            return;
        }
        
        var tri = new int[3 * (rayCount - 1)];
        for (int i = 1; i < rayCount - 1; i++)
        {
            tri[3*i] = 0;
            tri[3*i + 1] = i;
            tri[3*i + 2] = i - 1;
        }
        
        meshFilter.mesh.triangles = new int[] { };
        meshFilter.mesh.vertices = meshPositions.ToArray();
        meshFilter.mesh.triangles = tri;
    }
}
