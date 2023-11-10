using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    public bool active;
    public Camera activeCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        var targetPos = activeCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.value.x,
            Mouse.current.position.value.y,
            activeCamera.nearClipPlane));
        transform.position = new Vector3(targetPos.x, targetPos.y, 0);
    }
}
