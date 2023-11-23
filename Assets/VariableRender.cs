using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class VariableRender : MonoBehaviour
{
    [FormerlySerializedAs("targetMesh")] public Collider2D targetCollider;
    public List<Collider2D> targetVisBlockers = new ();
    public Vector2 cashedCenter;
    public Mesh cashedMesh;
    public bool observersCheckCenter = true;
    public List<Renderer> renderers = new ();
    public List<Light2D> lights = new ();
    public List<ShadowCaster2D> shadowCaster2Ds = new();
    public uint seenBy = 0;
    public uint showTo = 0;
    public bool onlyShowTo = false;

    private bool _wasObserved = false;

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnPreCullCallback;
        RenderPipelineManager.endCameraRendering += OnPostRenderCallback;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnPreCullCallback;
        RenderPipelineManager.endCameraRendering -= OnPostRenderCallback;
    }

    private void FixedUpdate()
    {
        seenBy = 0;
        if (!targetCollider) return;
        cashedMesh = targetCollider.CreateMesh(false, false);
        cashedCenter = targetCollider.transform.position;
    }

    public bool Observe(Vector3 pointFrom, ContactFilter2D potentialBlockers,  uint observer, int hitCount=1)
    {
        if (!targetCollider) return false;
        if (observersCheckCenter && CheckPoint(pointFrom, cashedCenter, potentialBlockers, observer, hitCount)) return true;
        foreach (var vertex in cashedMesh.vertices)
        {
            if (CheckPoint(pointFrom, vertex, potentialBlockers, observer, hitCount)) return true;
        }
        return false;
    }

    private bool CheckPoint(Vector3 pointFrom, Vector3 pointTo, ContactFilter2D potentialBlockers,  uint observer, int hitCount=1)
    {
        RaycastHit2D[] lineHits = new RaycastHit2D[hitCount];
        Physics2D.Linecast(pointFrom, pointTo, potentialBlockers, lineHits);
        foreach (var lineHit in lineHits)
        {
            if (lineHit.collider == targetCollider || targetVisBlockers.Contains(lineHit.collider))
            {
                seenBy |= observer;
                return true;
            }
        }
        return false;
    }

    public void OnPreCullCallback(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        var renderCamera = camera.GetComponent<VariableRenderCamera>();
        if (!renderCamera || (!onlyShowTo && (renderCamera.attachedPlayer & seenBy) > 0 )|| (renderCamera.attachedPlayer & showTo) > 0)
        {
            if (!_wasObserved)
                EnableAllRenderer();
            _wasObserved = true;
        }
    }

    public void EnableAllRenderer()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }

        foreach (var light in lights)
        {
            light.enabled = true;
        }

        foreach (var shadowCaster2D in shadowCaster2Ds)
        {
            shadowCaster2D.enabled = true;
            shadowCaster2D.Update();
        }
    }

    public void OnPostRenderCallback(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        if (!_wasObserved) return;
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
        
        foreach (var light in lights)
        {
            light.enabled = false;
        }

        foreach (var shadowCaster2D in shadowCaster2Ds)
        {
            shadowCaster2D.enabled = false;
            shadowCaster2D.Update();
        }
        _wasObserved = false;
    }
}
