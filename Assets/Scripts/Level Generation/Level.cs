using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all data of a finished level
/// </summary>
public class Level
{
    private Level() { }

    public static Level Current { get; set; } = new Level();

    public GameObject Root { get; private set; }
    public GameObject EnvironmentParent { get; private set; }
    public HashSet<Axial> TilePositions { get; private set; } = new HashSet<Axial>();
    public Dictionary<Axial, TileData> AllTiles { get; private set; } = new Dictionary<Axial, TileData>();

    public void AddEnvironment(GameObject environmentParent)
    {
        EnvironmentParent = environmentParent;
        EnvironmentParent.transform.SetParent(Root.transform, false);
    }
    public void AddBuildData(LevelBuildData buildData)
    {
        AllTiles = new Dictionary<Axial, TileData>(buildData.Tiles);
        TilePositions = new HashSet<Axial>(buildData.Tiles.Keys);
    }
    public static Level CreateWithRoot()
    {
        Level level = new Level();
        level.Root = new GameObject("Level Root");

        return level;
    }
}
