using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionHub : MonoBehaviour
{
    public HubAction move;
    public HubAction pickUp;
    public HubAction fire;

    [Serializable]
    public struct HubAction
    {
        public string name;
        public InputActionAsset playerInputAsset;

        public event Action<InputAction.CallbackContext> started
        {
            add => playerInputAsset.actionMaps[0][name].started += value;
            remove => playerInputAsset.actionMaps[0][name].started -= value;
        }
        
        public event Action<InputAction.CallbackContext> performed
        {
            add => playerInputAsset.actionMaps[0][name].performed += value;
            remove => playerInputAsset.actionMaps[0][name].performed -= value;
        }
        
        public event Action<InputAction.CallbackContext> canceled
        {
            add => playerInputAsset.actionMaps[0][name].canceled += value;
            remove => playerInputAsset.actionMaps[0][name].canceled -= value;
        }
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
