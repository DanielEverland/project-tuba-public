using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "TileDataGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Tile",
    order = 120)]
public sealed class TileDataGameEvent : GameEventBase<TileData>
{
}