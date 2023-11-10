using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RecoilManager : MonoBehaviour
{
    public RecoilPattern recoilPattern;

    public void ApplyRecoil()
    {
        var recoil = recoilPattern.GetNext();
        Mouse.current.WarpCursorPosition(Mouse.current.position.value + recoil);
    }
}
