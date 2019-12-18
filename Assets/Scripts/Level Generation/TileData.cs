using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

/// <summary>
/// Tile data is simply a data container for tiles
/// </summary>
[CreateAssetMenu(fileName = "Tile.asset", menuName = Utility.MenuItemRoot + "Tile", order = 210)]
public class TileData : ScriptableObject
{
    public BrushBase Brush => brush.Brush;
    public List<Material> BrushMaterials => brushMaterials;
    public Material EdgeMaterial => edgeMaterial;
    public bool CastsShadows => castsShadows;
    public bool IsWalkable => isWalkable;

    [SerializeField]
    private BrushData brush = default;
    [SerializeField]
    private List<Material> brushMaterials = default;
    [SerializeField]
    private Material edgeMaterial = default;
    [SerializeField]
    private bool castsShadows = false;
    [SerializeField]
    private bool isWalkable = true;

    #region Static
    public static List<TileData> AllTiles
    {
        get
        {
            if (allTiles == null)
            {
                allTiles = Resources.LoadAll<TileData>("Tiles").OrderBy(x => x.name).ToList();
            }

            return allTiles;
        }
    }
    private static List<TileData> allTiles;

    public static TileData Random
    {
        get
        {
            return AllTiles.Random();
        }        
    }
    #endregion
}