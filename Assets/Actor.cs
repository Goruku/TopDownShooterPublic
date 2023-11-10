using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
    
    public static Actor BindToClosestActor(Transform transform, out Actor actor)
    {
        Transform currentParent = transform.parent;
        actor = null;
        while (currentParent && !actor)
        { 
            actor = currentParent.GetComponent<Actor>();
            currentParent = currentParent.parent;
        }
        return actor;
    }
}
