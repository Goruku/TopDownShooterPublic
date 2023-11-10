using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "gun")]
public class Ammunition : ScriptableObject
{
    public Propellant propellant;
    public Caliber caliber;
    public float bulletRandomness;
    public List<GameObject> bullets;
}
