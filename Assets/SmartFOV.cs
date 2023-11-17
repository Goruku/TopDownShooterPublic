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

    public float distance = 15;
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
        
        foreach (var vertex in vertices)
        {
            var raycastHit2D = Physics2D.Linecast(currentPosition, vertex, layerMask);
            if (raycastHit2D.distance < circleCollider2D.radius)
                meshPositions.Add(raycastHit2D.point);
        }
        
        meshPositions = meshPositions.OrderBy(vertexPosition => Quaternion.LookRotation(Vector3.forward, vertexPosition - currentPosition).eulerAngles[2]).ToList();
        
        var rayCount = meshPositions.Count - 1;
        
        if (rayCount <= 0)
        {
            return;
        }
        
        var tri = new int[3 * rayCount];
        for (int i = 1; i < rayCount - 1; i++)
        {
            tri[3*i] = 0;
            tri[3*i + 1] = i;
            tri[3*i + 2] = i - 1;
        }

        if (circles)
        {
            tri[3*rayCount - 3] = 0;
            tri[3*rayCount - 2] = 1;
            tri[3 * rayCount - 1] = rayCount;
        }
        
        meshFilter.mesh.triangles = new int[] { };
        meshFilter.mesh.vertices = meshPositions.ToArray();
        meshFilter.mesh.triangles = tri;
    }
}
