using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a cave looking room
/// </summary>
public static class CaveRoom
{
    private const int Radius = 32;
    private const int Diameter = Radius * 2;
    private const int SmoothingSteps = 3;
    private const float FloorChance = 0.6f;

    // Despawn floor if it has fewer than this amout of adjacent floors
    private const int DespawnFloorCount = 3;

    // Spawn floor if empty spot has this amount or more adjacent floors
    private const int SpawnFloorCount = 5;

    private const int ConnectionPointCount = 4;
    private const float ConnectionPointMinDistance = 2;

    public static void CreateBaseLevel(LevelBuildData data, Theme theme)
    {
        CreateInitialData(data, theme);

        for (int i = 0; i < SmoothingSteps; i++)
        {
            PerformSmoothing(data, theme);
        }

        SelectLargestChunk(data);
    }
    private static void CreateInitialData(LevelBuildData data, Theme theme)
    {
        Utility.EvaluateAxialGrid(Vector2.zero, Diameter, Diameter, x =>
        {
            bool shouldSpawn = Random.Range(0f, 1f) <= FloorChance;

            if (shouldSpawn)
                data.Tiles.Add(x, theme.FloorTile);
        });
    }
    private static void PerformSmoothing(LevelBuildData data, Theme theme)
    {
        Utility.EvaluateAxialGrid(Vector2.zero, Diameter, Diameter, x =>
        {
            if (data.Tiles.ContainsKey(x))
            {
                ShouldDespawn(x, data);
            }
            else
            {
                ShouldSpawn(x, data, theme);
            }
        });
    }
    private static void ShouldDespawn(Axial position, LevelBuildData data)
    {
        int adjacentCount = 0;

        Utility.AdjacentHexagons(position, w =>
        {
            if (data.Tiles.ContainsKey(w))
            {
                adjacentCount++;
            }
        });

        if (adjacentCount < DespawnFloorCount)
            data.Tiles.Remove(position);
    }
    private static void ShouldSpawn(Axial position, LevelBuildData data, Theme theme)
    {
        int adjacentCount = 0;

        Utility.AdjacentHexagons(position, w =>
        {
            if (data.Tiles.ContainsKey(w))
            {
                adjacentCount++;
            }
        });

        if (adjacentCount >= SpawnFloorCount)
            data.Tiles.Add(position, theme.FloorTile);
    }
    /// <summary>
    /// Evaluate all the chunks and select the one that's largest
    /// </summary>
    private static void SelectLargestChunk(LevelBuildData data)
    {
        Dictionary<Axial, TileData> largestChunk = null;

        while (data.Tiles.Count > 0)
        {
            Dictionary<Axial, TileData> currentChunk = GetChunk();

            if(largestChunk == null)
            {
                largestChunk = currentChunk;
                continue;
            }

            if (currentChunk.Count > largestChunk.Count)
                largestChunk = currentChunk;
        }

        if (largestChunk == null)
            throw new System.InvalidOperationException("No chunk was selected!");

        data.Tiles = largestChunk;

        Dictionary<Axial, TileData> GetChunk()
        {
            Dictionary<Axial, TileData> currentChunk = new Dictionary<Axial, TileData>();
            Queue<KeyValuePair<Axial, TileData>> queue = new Queue<KeyValuePair<Axial, TileData>>();
            queue.Enqueue(data.Tiles.Random());

            while (queue.Count > 0)
            {
                KeyValuePair<Axial, TileData> currentEntry = queue.Dequeue();

                if (!data.Tiles.ContainsKey(currentEntry.Key))
                    continue;

                data.Tiles.Remove(currentEntry.Key);
                currentChunk.Add(currentEntry.Key, currentEntry.Value);

                Utility.AdjacentHexagons(currentEntry.Key, x =>
                {
                    if (data.Tiles.ContainsKey(x))
                        queue.Enqueue(new KeyValuePair<Axial, TileData>(x, data.Tiles[x]));
                });
            }

            return currentChunk;
        }
    }
}