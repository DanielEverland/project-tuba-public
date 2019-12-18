using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable data container for rooms
/// </summary>
[System.Serializable]
public class RoomData : ScriptableObject, ISerializationCallbackReceiver, ILevelEntity
{
    public TileData this[Axial index]
    {
        get
        {
            return Tiles[index];
        }
        set
        {
            Tiles[index] = value;
        }
    }
    
    /// <summary>
    /// The positions of the room
    /// </summary>
    public Axial Position { get; set; }

    /// <summary>
    /// Contains the data of the room.
    /// </summary>
    public Dictionary<Axial, TileData> Tiles { get; set; } = new Dictionary<Axial, TileData>();

    /// <summary>
    /// Contains all prefabs in the room. These will be instantiated when the room is placed in the level
    /// </summary>
    public Dictionary<Axial, GameObject> Prefabs { get; set; } = new Dictionary<Axial, GameObject>();

    /// <summary>
    /// Rooms must be connected to other rooms using their connection point
    /// </summary>
    public List<Axial> ConnectionPoints => connectionPoints;

    [SerializeField]
    private List<Axial> connectionPoints = new List<Axial>();
    [SerializeField]
    private List<Axial> prefabKeys = new List<Axial>();
    [SerializeField]
    private List<GameObject> prefabValues = new List<GameObject>();
    [SerializeField]
    private List<Axial> tileKeys = new List<Axial>();
    [SerializeField]
    private List<TileData> tileValues = new List<TileData>();

    public void AddToLevel(LevelBuildData levelData)
    {
        if(levelData.Tiles.Count == 0)
        {
            PlaceOnLevel(levelData, Axial.zero);
            return;
        }

        // Get all tiles in level which have an available neighbor
        LinkedList<Axial> edgeTiles = new LinkedList<Axial>();
        foreach (Axial tilePosition in levelData.Tiles.Keys)
        {
            foreach (Axial direction in AxialDirection.AllDirections)
            {
                if (!levelData.Tiles.ContainsKey(tilePosition + direction))
                {
                    edgeTiles.AddLast(tilePosition);
                    break;
                }
            }
        }

        List<Axial> availableConnectionPoints = new List<Axial>(ConnectionPoints);
        while (availableConnectionPoints.Count > 0)
        {
            Queue<Axial> edgeTilesQueue = new Queue<Axial>(edgeTiles);

            Axial connectionPoint = availableConnectionPoints.Random();
            availableConnectionPoints.Remove(connectionPoint);

            // Figure out which directions are adjacent to the connection point
            List<Axial> availableAdjacencyDirections = new List<Axial>();

            Utility.AdjacentHexagons(connectionPoint, x =>
            {
                if (!Tiles.ContainsKey(x))
                {
                    availableAdjacencyDirections.Add(x);
                }
            });

            while (edgeTilesQueue.Count > 0)
            {
                Axial edgeTileCandidate = edgeTilesQueue.Dequeue();

                foreach (Axial adjacentDirection in availableAdjacencyDirections)
                {
                    Axial offset = edgeTileCandidate - adjacentDirection;

                    if (Fits(levelData, offset))
                    {
                        PlaceOnLevel(levelData, offset);
                        return;
                    }
                }
            }
        }

        throw new System.InvalidOperationException("Unable to place room");
    }
    private void PlaceOnLevel(LevelBuildData levelData, Axial offset)
    {
        Position = offset;

        foreach (var pair in Tiles)
        {
            levelData.AddTile(pair.Key + offset, pair.Value);
        }

        foreach (var pair in Prefabs)
        {
            levelData.AddPrefab(pair.Key + offset, pair.Value);
        }
    }
    private bool Fits(LevelBuildData levelData, Axial offset)
    {
        bool isAdjacent = false;
        foreach (Axial connectionPoint in ConnectionPoints)
        {
            Utility.AdjacentHexagons(connectionPoint + offset, x =>
            {
                if (levelData.Tiles.ContainsKey(x))
                    isAdjacent = true;
            });

            if (!isAdjacent)
                return false;
        }

        bool anyOverlap = false;
        foreach (Axial position in Tiles.Keys)
        {
            Axial actualPosition = position + offset;

            if (levelData.Tiles.ContainsKey(actualPosition))
            {
                anyOverlap = true;
                break;
            }
        }

        return !anyOverlap;
    }
    /// <summary>
    /// Checks whether the connection point is valid.
    /// Will throw an exception if it's not
    /// </summary>
    public void VerifyConnectionPointValidity()
    {
        if (ConnectionPoints.Count == 0)
            throw new System.NullReferenceException($"No connection points for {name}!");

        foreach (Axial connectionPoint in ConnectionPoints)
        {
            if (!IsPositionValidConnetionPoint(connectionPoint))
            {
                throw new System.InvalidOperationException($"Room {name} with connection point = {connectionPoint} is invalid!");
            }
        }
    }
    private bool IsPositionValidConnetionPoint(Axial point)
    {
        bool isValid = false;

        Utility.AdjacentHexagons(point, x =>
        {
            if (!Tiles.ContainsKey(x))
            {
                isValid = true;
            }
        });

        return isValid;
    }
    public void OnBeforeSerialize()
    {
        // Tiles.
        tileKeys.Clear();
        tileValues.Clear();

        foreach (var keyValuePair in Tiles)
        {
            tileKeys.Add(keyValuePair.Key);
            tileValues.Add(keyValuePair.Value);
        }

        // Prefabs.
        prefabKeys.Clear();
        prefabValues.Clear();

        foreach (var keyValuePair in Prefabs)
        {
            prefabKeys.Add(keyValuePair.Key);
            prefabValues.Add(keyValuePair.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        // Tiles.
        Tiles = new Dictionary<Axial, TileData>();

        for (int i = 0; i < Mathf.Min(tileKeys.Count, tileValues.Count); i++)
        {
            Tiles.Add(tileKeys[i], tileValues[i]);
        }

        // Prefabs.
        Prefabs = new Dictionary<Axial, GameObject>();

        for (int i = 0; i < Mathf.Min(prefabKeys.Count, prefabValues.Count); i++)
        {
            Prefabs.Add(prefabKeys[i], prefabValues[i]);
        }
    }
    public static RoomData Create()
    {
        return ScriptableObject.CreateInstance<RoomData>();
    }
}
