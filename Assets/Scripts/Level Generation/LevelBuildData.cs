using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the data necessary to build a level
/// </summary>
public class LevelBuildData
{
    public Vector2 SpawnPosition { get; set; }

    public Dictionary<Axial, TileData> Tiles { get; set; } = new Dictionary<Axial, TileData>();
    public Dictionary<Axial, GameObject> Prefabs { get; set; } = new Dictionary<Axial, GameObject>();
    
    public void AddTile(Axial position, TileData tile)
    {
        if(Tiles.ContainsKey(position))
            Debug.LogError($"Attempted to overwrite tile at position {position}");

        Tiles.Add(position, tile);
    }
    public void AddPrefab(Axial position, GameObject prefab)
    {
        if(Prefabs.ContainsKey(position))
            Debug.LogError($"Attempted to overwrite prefab at position {position}");

        Prefabs.Add(position, prefab);
    }
}
