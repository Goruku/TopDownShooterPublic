using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class PathMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public List<Collider2D> pathColliders;
    public Collider2D triggerCollider;

    public bool random;

    public float acceleration;
    public float maxSpeed = 2;

    public int currentTarget;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentVelocity = _rigidbody2D.velocity;
        
        var targetingVector = pathColliders[currentTarget].transform.position - transform.position;

        if (currentVelocity.magnitude < maxSpeed)
            _rigidbody2D.velocity += (Vector2) targetingVector.normalized * acceleration;

        Collider2D[] contacts = new Collider2D[5];

        if (!triggerCollider) return;
        triggerCollider.GetContacts(contacts);
        var hasReachedDestination = contacts.Contains(pathColliders[currentTarget]);
        
        if (!hasReachedDestination) return;
        
        if (random)
        {
            currentTarget = Random.Range(0, pathColliders.Count);
        }
        else
        {
            currentTarget = (currentTarget >= pathColliders.Count - 1) ? 0 : currentTarget + 1;
        }
    }
}
