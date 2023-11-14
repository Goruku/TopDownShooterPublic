using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GunPart : MonoBehaviour
{
    public float velocityMultiplier = 1;
    public GunFrame gunFrame;

    public void AttachToGunFrame()
    {
        Entity.BindToClosest<GunFrame>(transform, out gunFrame);
    }
    
    private void Reset()
    {
        AttachToGunFrame();
        if (!gunFrame)
        {
            Debug.LogError("A GunPart requires a parent GunFrame or needs to have a GunFrame itself");
            DestroyImmediate(this);
            return;
        }
    }
    
    protected void OnEnable()
    {
        AttachToGunFrame();
    }

    protected void OnTransformParentChanged()
    {
        OnDisable();
        OnEnable();
    }

    protected void OnDisable()
    {
        AttachToGunFrame();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
