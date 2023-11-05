using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "gun")]
public class Ammunition : ScriptableObject
{
    public Propellant propellant;
    public Caliber caliber;
    [FormerlySerializedAs("bullet")] public List<GameObject> bullets;
}
