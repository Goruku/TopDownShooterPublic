using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHammerInfo : MonoBehaviour
{
    public Image hammerStateImage;
    public Color pulledColor = Color.white;
    public Color releasedColor = Color.black;

    public void UpdateHammerState(bool hammerState)
    {
        hammerStateImage.color = hammerState ? pulledColor : releasedColor;
    }
}
