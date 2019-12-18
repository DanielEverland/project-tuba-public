using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A theme contains references to all data objects used to generate a level
/// It's used to define the appearance of a certain level
/// </summary>
[CreateAssetMenu(menuName = Utility.MenuItemRoot + "Theme", order = 400)]
public class Theme : ScriptableObject
{
    [SerializeField]
    private TileData floorTile = default;
    [SerializeField]
    private TileData wallTile = default;
    [SerializeField]
    private RoomData spawnRoom = default;
    [SerializeField]
    private RoomData bossCatwalk = default;
    [SerializeField]
    private List<EntityCollection> enemyCombinations = default;

    public TileData FloorTile => floorTile;
    public TileData WallTile => wallTile;
    public RoomData SpawnRoom => spawnRoom;
    public RoomData BossCatwalk => bossCatwalk;
    public IReadOnlyList<EntityCollection> EnemyCombinations => enemyCombinations;
}
