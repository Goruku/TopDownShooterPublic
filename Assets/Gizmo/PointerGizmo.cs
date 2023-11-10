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
        _shootingManager.duringShot += CreatePointer;
    }

    private void OnDisable()
    {
        _shootingManager.duringShot -= CreatePointer;
    }

    private void CreatePointer(Transform location)
    {
        if (pointerLocation)
        {
            Instantiate(pointerPrefab, pointerLocation.position, pointerLocation.rotation);
        }
    }
}
