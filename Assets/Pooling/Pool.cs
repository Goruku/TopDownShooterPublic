using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Pool : MonoBehaviour, ISerializationCallbackReceiver
{
    public PoolablePrefab.PoolId poolId;
    
    [SerializeField]
    protected List<PoolablePrefab> _poolablePrefabs = new ();

    public Queue<PoolablePrefab> poolablePrefabs = new ();

    public virtual PoolablePrefab getFirstAvailable()
    {
        PoolablePrefab poolablePrefab = poolablePrefabs.TryDequeue(out poolablePrefab) ? poolablePrefab : null;
        if (poolablePrefab)
        {
            poolablePrefab.gameObject.SetActive(true);
        }
        return poolablePrefab;
    }

    public void OnBeforeSerialize()
    {
        _poolablePrefabs = new List<PoolablePrefab>(poolablePrefabs);
    }

    public void OnAfterDeserialize()
    {
        poolablePrefabs = new Queue<PoolablePrefab>(_poolablePrefabs);
    }
}
