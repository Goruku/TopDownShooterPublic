using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour, ISerializationCallbackReceiver
{
    
    public List<PoolablePrefab.PoolId> _activePoolKeys = new ();
    public List<Pool> _activePoolValues = new ();
    
    public Dictionary<PoolablePrefab.PoolId, Pool>  activePools = new ();

    public void OnBeforeSerialize()
    {
        _activePoolKeys.Clear();
        _activePoolValues.Clear();

        foreach (var activePool in activePools)
        {
            _activePoolKeys.Add(activePool.Key);
            _activePoolValues.Add(activePool.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        activePools.Clear();

        for (int i = 0; i < _activePoolKeys.Count; i++)
        {
            if (i >= _activePoolValues.Count)
            {
                if (activePools.ContainsKey(_activePoolKeys[i]))
                {
                    Debug.LogWarning("Tried adding an existing key, replacing by EDIT_ONLY instead");
                    activePools.Add(PoolablePrefab.PoolId.EDIT_ONLY, null);
                }
                else
                {
                    activePools.Add(_activePoolKeys[i], null);
                }
            }
            else
            {
                activePools.Add(_activePoolKeys[i], _activePoolValues[i]);
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
