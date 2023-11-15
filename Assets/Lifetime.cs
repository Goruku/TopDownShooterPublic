using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime;
    public float lifeEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeEnd = Time.time + lifetime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > lifeEnd) Destroy(gameObject);
    }
}
