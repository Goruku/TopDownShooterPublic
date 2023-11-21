using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VariableRenderObserver : MonoBehaviour
{
    public uint attachedPlayer;

    protected abstract void FlagObjectsForRender();
}
