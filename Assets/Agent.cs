using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Agent : Actor
{
    private PlayerInput _playerInput;
    
    public PlayerInput playerInput
    {
        get {
            if (!_playerInput)
            {
                _playerInput = GetComponent<PlayerInput>();
            }
            return _playerInput;
        }
    }
}
