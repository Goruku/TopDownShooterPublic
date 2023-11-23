using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class ShadowCasterTileSnapper : MonoBehaviour
{
    public bool active = false;
    public float gridSize = 1f;
    public float cooldown = 0.15f;
    private float _lastCooldown = 0;

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (Time.time < _lastCooldown + cooldown) return;
        _lastCooldown = Time.time;
        var shadowCasters = GetComponents<ShadowCaster2D>();
        foreach (var shadowCaster in shadowCasters)
        {
            var newPath = shadowCaster.shapePath;
            for (int i = 0; i < newPath.Length; i++)
            {
                newPath[i] = new Vector3(Mathf.Round(newPath[i].x * gridSize) / gridSize, Mathf.Round(newPath[i].y*gridSize)/gridSize, Mathf.Round(newPath[i].z*gridSize)/gridSize);
            }
            SetPath(shadowCaster, newPath);
        }
    }
    
    public static void SetPath(ShadowCaster2D shadowCaster, Vector3[] path)
    {
        FieldInfo shapeField = typeof(ShadowCaster2D).GetField("m_ShapePath",
            BindingFlags.NonPublic |
            BindingFlags.Instance);
        shapeField.SetValue(shadowCaster, path);
    }
}
