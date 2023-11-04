using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity: MonoBehaviour
{

    public static T BindToClosest<T>(Transform transform, out T entity) where T : Entity {
        Transform currentParent = transform;
        entity = null;
        while (currentParent && !entity)
        { 
            entity = currentParent.GetComponent<T>();
            currentParent = currentParent.parent;
        }
        return entity;
    }
}
