using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolablePrefab : MonoBehaviour
{
    public PoolId poolId;
    public Pool associatedPool;

    [SerializeField]
    public enum PoolId
    {
        Bullet,
        Laser,
        EDIT_ONLY
    }

    public void Release()
    {
        if (associatedPool)
            associatedPool.poolablePrefabs.Enqueue(this);
        else
            Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
