using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootingManager))]
public class PointerGizmo : MonoBehaviour
{

    private ShootingManager _shootingManager;
    public Transform pointerLocation;
    public GameObject pointerPrefab;

    private void OnEnable()
    {
        _shootingManager = GetComponent<ShootingManager>();
        _shootingManager.shotTrigger += CreatePointer;
        Actor actor;
        Entity.BindToClosest<Actor>(transform, out actor);
        if (actor)
        {
            pointerLocation = actor.GetComponent<PhysicsTargeter>().target;
        }
    }

    private void OnDisable()
    {
        _shootingManager.shotTrigger -= CreatePointer;
    }

    private void CreatePointer(Transform location)
    {
        if (pointerLocation)
        {
            Instantiate(pointerPrefab, pointerLocation.position, pointerLocation.rotation);
        }
    }
}
