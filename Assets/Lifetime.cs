using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField]
    private PoolablePrefab poolablePrefab;
    
    public float lifetime;
    public float lifeEnd;

    private void OnEnable()
    {
        lifeEnd = Time.time + lifetime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < lifeEnd) return;

        if (poolablePrefab)
        {
            poolablePrefab.Release();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
