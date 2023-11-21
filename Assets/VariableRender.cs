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
    public uint seenBy = 0;
    public uint showTo = 0;
    public bool onlyShowTo = false;

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
    }

    public void OnPreCullCallback(ScriptableRenderContext scriptableRenderContext, Camera camera)
    {
        var renderCamera = camera.GetComponent<VariableRenderCamera>();
        if (!renderCamera || (!onlyShowTo && (renderCamera.attachedPlayer & seenBy) > 0 )|| (renderCamera.attachedPlayer & showTo) > 0)
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
