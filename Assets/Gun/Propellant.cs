using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gun")]
public class Propellant : ScriptableObject
{
    public float pushForce;

    public float EffectivePush()
    {
        return pushForce;
    }
}
