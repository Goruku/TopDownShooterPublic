using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AimingSensitivityProfile : ScriptableObject
{
    public float rotationMultiplier = 1;
    public AnimationCurve rotationCurve;
    public float distanceMultiplier = 1;
    public AnimationCurve distanceCurve;
}
