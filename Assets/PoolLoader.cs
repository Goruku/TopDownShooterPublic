using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLoader : MonoBehaviour
{
    [SerializeField]
    private PoolManagerProvider PoolManagerProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Loading PoolManager At Least Once: " + PoolManagerProvider.PoolManager);
    }
}
