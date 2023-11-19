using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VariableRenderCamera : MonoBehaviour
{
    public uint attachedPlayer;
    public bool seesAll = false;
}
