using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class VariableRender : MonoBehaviour
{
    public List<Renderer> renderers = new ();
    public uint seenBy;
    public uint onlyObservableBy;

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

    private void Update()
    {
        seenBy = 0;
    }
    
    public void OnPreCullCallback(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        var renderCamera = camera.GetComponent<VariableRenderCamera>();
        if (!renderCamera)
        {
            EnableAllRenderer();
            return;
        }
        if ((renderCamera.attachedPlayer & seenBy) > 0 && (onlyObservableBy == 0 || (renderCamera.attachedPlayer & onlyObservableBy) > 0 ))
        {
            EnableAllRenderer();
        }
    }

    public void EnableAllRenderer()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    public void OnPostRenderCallback(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
