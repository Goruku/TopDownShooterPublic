using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPool : GrowingPool
{
    [SerializeField]
    private int maximumHibernateCount;

    [SerializeField]
    private float timeToHibernate;

    private float lastFetch;
    
    
    public void FixedUpdate()
    {
        if (lastFetch + timeToHibernate >= Time.time)
        {
            while (poolablePrefabs.Count  > maximumHibernateCount)
            {
                var poolablePrefab = poolablePrefabs.Dequeue();
                Destroy(poolablePrefab.gameObject);
            }
        }
    }

    public override PoolablePrefab getFirstAvailable()
    {
        lastFetch = Time.time;
        return base.getFirstAvailable();
    }
}
