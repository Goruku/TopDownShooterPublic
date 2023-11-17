using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDebugger : MonoBehaviour
{
    public List<Vector3> cashedVertices;
    private void Start()
    {
        GetComponent<TilemapCollider2D>().CreateMesh(true, true).GetVertices(cashedVertices);
        Debug.Log(cashedVertices);
        Debug.Log(cashedVertices.Count);
    }
}
