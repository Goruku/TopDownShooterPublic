using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionHub : MonoBehaviour
{
    public HubAction move;
    public HubAction pickUp;

    [Serializable]
    public struct HubAction
    {
        public string name;
        public InputActionAsset playerInputAsset;

        public void AddStarted(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].started += action;
        }

        public void RemoveStarted(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].started -= action;
        }
        
        public void AddPerformed(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].performed += action;
        }
        public void RemovePerformed(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].performed -= action;
        }
        public void AddCanceled(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].canceled += action;
        }
        public void RemoveCanceled(Action<InputAction.CallbackContext> action)
        {
            playerInputAsset.actionMaps[0][name].canceled -= action;
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
