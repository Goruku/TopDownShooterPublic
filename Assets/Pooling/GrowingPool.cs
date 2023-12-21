using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingPool : Pool
{
    public GameObject fallbackPrefab;
    
    public override PoolablePrefab getFirstAvailable()
    {
        var poolablePrefab = base.getFirstAvailable();
        if (!poolablePrefab)
        { 
            poolablePrefab = Instantiate(fallbackPrefab, transform).GetComponent<PoolablePrefab>();
            _poolablePrefabs.Add(poolablePrefab);
            poolablePrefab.associatedPool = this;
        }
        return poolablePrefab;
    }
}
