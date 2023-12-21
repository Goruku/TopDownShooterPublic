using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PoolManagerProvider : ScriptableObject
{

    [SerializeField]
    private PoolManager _poolManager;
    public GameObject poolManagerPrefab;
    
    public PoolManager PoolManager
    {
        get => fetchOrCreateInstance();
        set => _poolManager = value;
    }


    public PoolManager fetchOrCreateInstance()
    {
        if (_poolManager)
            return _poolManager;
        
        GameObject gameObject = GameObject.FindWithTag("PoolManager");
        if (!gameObject)
        {
            var poolManager = Instantiate(poolManagerPrefab);
            DontDestroyOnLoad(poolManager);
        }
        else
        {
            _poolManager = gameObject.GetComponent<PoolManager>();
        }
        return _poolManager;
    }
}
