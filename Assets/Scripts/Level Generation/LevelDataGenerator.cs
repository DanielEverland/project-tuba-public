using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

/// <summary>
/// Creates the underlying data for a level
/// </summary>
public static class LevelDataGenerator
{
    private static Theme currentTheme = default;
    private static LevelBuildData levelBuildData = default;

    private const int RoomsToSpawn = 2;
    private const int RoomPlacementAttempts = 1;
    private const float EnemyMinSpawnDistance = 20;
    private const float MinSpawnablePositions = 4;
    private const float SpawnablePositionsSlope = 2.5f;
    
    public static LevelBuildData CreateNewLevel(Theme theme)
    {
        currentTheme = theme;
        levelBuildData = new LevelBuildData();

        CaveRoom.CreateBaseLevel(levelBuildData, theme);
        SpawnEnemies();

        AddSpawnRoom();
        AddBossCatwalk();

        return levelBuildData;
    }
    private static void AddSpawnRoom()
    {
        RoomData spawnRoom = Object.Instantiate(currentTheme.SpawnRoom);
        spawnRoom.AddToLevel(levelBuildData);
        
        levelBuildData.SpawnPosition = Utility.AxialToWorldPosition(spawnRoom.Position);
    }
    private static void AddBossCatwalk()
    {
        RoomData bossCatwalk = Object.Instantiate(currentTheme.BossCatwalk);
        bossCatwalk.AddToLevel(levelBuildData);
    }
    private static void SpawnEnemies()
    {
        List<Axial> blacklistedPositions = new List<Axial>();

        foreach (Axial tilePosition in levelBuildData.Tiles.Keys)
        {
            if(!IsPositionTooCloseToOtherEnemies(tilePosition))
            {
                blacklistedPositions.Add(tilePosition);

                SpawnEnemiesAtPosition(tilePosition, currentTheme.EnemyCombinations.Random());
            }
        }

        bool IsPositionTooCloseToOtherEnemies(Axial position)
        {
            if (!Utility.IsAxialPositionWalkable(levelBuildData.Tiles, position))
                return true;

            bool canSpawn = true;

            for (int i = 0; i < blacklistedPositions.Count; i++)
            {
                if (Vector2.Distance(blacklistedPositions[i], position) < EnemyMinSpawnDistance)
                    canSpawn = false;
            }

            return !canSpawn;
        }
    }
    private static void SpawnEnemiesAtPosition(Axial center, EntityCollection collection)
    {
        List<Axial> spawnablePositions = GetSpawnablePositions(center, collection.Count);

        foreach (Entity prefab in collection)
        {
            Axial position = spawnablePositions.Random();
            spawnablePositions.Remove(position);

            Entity instance = Entity.Instantiate(prefab);
            instance.transform.position = Utility.AxialToWorldPosition(position);
        }
    }
    private static List<Axial> GetSpawnablePositions(Axial center, int requiredEnemies)
    {
        int requiredMinSpawnable = Mathf.RoundToInt(SpawnablePositionsSlope * requiredEnemies + MinSpawnablePositions);

        List<Axial> viablePositions = new List<Axial>();
        Queue<Axial> positionsToCheck = new Queue<Axial>();
        positionsToCheck.Enqueue(center);

        while (viablePositions.Count < requiredMinSpawnable)
        {
            Axial currentPosition = positionsToCheck.Dequeue();

            foreach (AxialDirection direction in AxialDirection.AllDirections)
            {
                Axial neighborPosition = currentPosition + direction;

                if (Utility.IsAxialPositionWalkable(levelBuildData.Tiles, neighborPosition) && !viablePositions.Contains(neighborPosition))
                {
                    positionsToCheck.Enqueue(neighborPosition);
                    viablePositions.Add(neighborPosition);
                }
            }
        }

        return viablePositions;
    }
}
