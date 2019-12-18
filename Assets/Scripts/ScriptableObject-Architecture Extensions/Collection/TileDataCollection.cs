using UnityEngine;

[CreateAssetMenu(
    fileName = "TileDataCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Tile",
    order = 120)]
public class TileDataCollection : Collection<TileData>
{
}