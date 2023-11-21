using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VariableRenderBrain : MonoBehaviour
{
    public List<VariableRenderObserver> variableRenderObservers;
    public List<VariableRenderCamera> variableRenderCameras;
    public List<VariableRender> variableRenderTargets;
    public List<VariableRender> showToTargets;

    public void SetObserverPlayer(uint attachedPlayer)
    {
        foreach (var variableRenderObserver in variableRenderObservers)
        {
            variableRenderObserver.attachedPlayer = attachedPlayer;
        }
    }

    public void SetShowTo(uint showTo)
    {
        foreach (var showToTarget in showToTargets)
        {
            showToTarget.showTo = showTo;
        }
    }

    public void SetShowOnly(bool onlyShowTo)
    {
        foreach (var showToTarget in showToTargets)
        {
            showToTarget.onlyShowTo = onlyShowTo;
        }
    }

    public void SetCameraPlayer(uint attachedPlayer)
    {
        foreach (var variableRenderCamera in variableRenderCameras)
        {
            variableRenderCamera.attachedPlayer = attachedPlayer;
        }
    }

    public void SetName(string name)
    {
        foreach (var variableRenderObserver in variableRenderObservers)
        {
            variableRenderObserver.name = "VariableRenderObserver (" + name + ")";
        }

        foreach (var variableRenderCamera in variableRenderCameras)
        {
            variableRenderCamera.name = "VariableRenderCamera (" + name + ")";
        }

        foreach (var variableRenderTarget in variableRenderTargets)
        {
            variableRenderTarget.name = "VariableRenderTarget (" + name + ")";
        }
    }

}
