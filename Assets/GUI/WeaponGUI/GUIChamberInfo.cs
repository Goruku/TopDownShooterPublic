using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GUIChamberInfo : MonoBehaviour
{
    public Image chamberStateImage;
    public Color chamberedColor = Color.white;
    public Color emptyColor = Color.black;

    public void UpdateChamberState(bool empty)
    {
        chamberStateImage.color = empty ? emptyColor : chamberedColor;
    }
}
