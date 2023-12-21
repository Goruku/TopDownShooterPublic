using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour, ISerializationCallbackReceiver
{
    
    public Dictionary<PoolablePrefab.PoolId, Pool> activePools = new ();

    public List<Pool> _activePools = new ();
    
    public void OnBeforeSerialize()
    {
        _activePools.Clear();
        foreach (var activePool in activePools.Values)
        {
            _activePools.Add(activePool);
        }
    }

    public void OnAfterDeserialize()
    {
        activePools.Clear();
        foreach (var activePool in _activePools)
        {
            if (!activePool)
            {
                activePools.Add(PoolablePrefab.PoolId.NULL, activePool);
            }
            else
            {
                if (activePools.ContainsKey(activePool.poolId))
                {
                    Debug.LogWarning("Pool id was already present, attempted to add Null, None instead");
                    activePools.Add(PoolablePrefab.PoolId.NULL, null);
                }
                else
                {
                    activePools.Add(activePool.poolId, activePool);
                }
            }
        }
    }

    /*
     *Provides an object from the pool or null if it wasn't possible 
     */
    public GameObject FetchByPoolId(PoolablePrefab.PoolId poolId)
    {
        Debug.Log("Fetched from pool " + poolId);
        var poolablePrefab = activePools[poolId].getFirstAvailable();
        return poolablePrefab ? poolablePrefab.gameObject : null;
    }
}
