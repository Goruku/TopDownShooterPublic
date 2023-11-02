using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtable : MonoBehaviour
{

    public delegate void Damaged(float damage);

    public Damaged damaged = damage => {};
    public float damageMultiplier = 1;
    public float minDamage;
    public float maxDamage = float.MaxValue;

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D[] colliders = new ContactPoint2D[5];
        other.GetContacts(colliders);
        var damage = colliders[0].normalImpulse * damageMultiplier;
        if (damage < minDamage)
            damage = 0;
        damage = Mathf.Clamp(damage, 0, maxDamage);
        Debug.Log(damage);
        damaged(damage);
    }
}
