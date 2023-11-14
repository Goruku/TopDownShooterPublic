using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellantTank : GunPart
{
    
    public delegate void PropellantEvent(Propellant propellant, GunFrame.GunState gunState);
}
