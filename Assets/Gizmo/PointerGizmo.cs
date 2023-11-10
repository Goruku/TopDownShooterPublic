using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunTrigger))]
public class PointerGizmo : MonoBehaviour
{

    private GunTrigger _gunTrigger;
    public Transform pointerLocation;
    public GameObject pointerPrefab;

    private void OnEnable()
    {
        _gunTrigger = GetComponent<GunTrigger>();
        //_gunTrigger.performed += CreatePointer;
        Actor actor;
        Entity.BindToClosest<Actor>(transform, out actor);
        if (actor)
        {
            pointerLocation = actor.GetComponent<PhysicsTargeter>().target;
        }
    }

    private void OnDisable()
    {
        //_gunTrigger.performed -= CreatePointer;
    }

    private void CreatePointer(Transform location)
    {
        if (pointerLocation)
        {
            Instantiate(pointerPrefab, pointerLocation.position, pointerLocation.rotation);
        }
    }
}
