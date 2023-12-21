using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class SmartFOV : MonoBehaviour, ISerializationCallbackReceiver
{
    public CircleCollider2D circleCollider2D;
    public ContactFilter2D visionBlockFilter;
    public Light2D visionCone;
    public Transform anchor;

    public float innerAngle = 0;
    public float angle = 90;
    public float radius = 5;
    public uint attachedPlayer;
    public ContactFilter2D viewPointRenderContactFilter = new ContactFilter2D(){useLayerMask = true, useTriggers = true};
    public ContactFilter2D potentialBlockers;
    public Collider2D[] colliders;

    public bool shouldUpdateFOVLight = false;
    
    public int maxColliderCount = 50;
    
    private void Start()
    {
        
    }

    private void Reset()
    {
        visionCone = GetComponent<Light2D>();
        UpdateFOVLight();
    }

    private void UpdateFOVLight()
    {
        circleCollider2D.radius = radius;
        visionCone.pointLightInnerAngle = innerAngle;
        visionCone.pointLightOuterAngle = angle;
        visionCone.pointLightInnerRadius = circleCollider2D.radius;
        visionCone.pointLightOuterRadius = circleCollider2D.radius;
    }

    private void FixedUpdate()
    {
        if (shouldUpdateFOVLight)
        {
            UpdateFOVLight();
            shouldUpdateFOVLight = false;
        }
        
        colliders = new Collider2D[maxColliderCount];
        Physics2D.OverlapCollider(circleCollider2D, viewPointRenderContactFilter, colliders);
        foreach (var collider in colliders)
        {
            if (!collider) break;
            var variableRender = collider.GetComponent<VariableRender>();
            if (variableRender)
            {
                variableRender.Observe(anchor.position, potentialBlockers, attachedPlayer);
            }
        }
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        shouldUpdateFOVLight = true;
    }
}
