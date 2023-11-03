using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabber : MonoBehaviour
{
    
    public PlayerActionHub playerActionHub;
    public Collider2D pickupHitbox;
    public GunMounter gunMounter;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        playerActionHub.pickUp.AddPerformed(Grab);
    }

    private void OnDisable()
    {
        playerActionHub.pickUp.RemovePerformed(Grab);
    }

    void Grab(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Hi, I tried to grab");
        Collider2D[] colliders = new Collider2D[100];
        pickupHitbox.GetContacts(colliders);
        foreach (var collider in colliders)
        {
            if (!collider) continue;
            var shootingManager = collider.GetComponent<ShootingManager>();
            if (shootingManager && !shootingManager.owner)
            {
                Debug.Log("You tried to pick up a gun!");
                gunMounter.SwapGun(shootingManager);
            }
        }
    }
}
