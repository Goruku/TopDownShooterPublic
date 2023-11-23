using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilePaletteController : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<TilemapLayerInfo> tileMapLayers;
    public bool shouldUpdateColor = false;
    
    
    [Serializable]
    public struct TilemapLayerInfo
    {
        public Color color;
        public List<Tilemap> tilemaps;
    }

    private void Update()
    {
        if (!shouldUpdateColor) return;
        UpdateColor();
        shouldUpdateColor = false;
    }

    public void UpdateColor()
    {
        foreach (var tilemapLayerInfo in tileMapLayers)
        {
            foreach (var tilemap in tilemapLayerInfo.tilemaps)
            {
                tilemap.color = tilemapLayerInfo.color;
            }
        }
    }

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        shouldUpdateColor = true;
    }
}
