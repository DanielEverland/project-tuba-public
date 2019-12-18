using UnityEngine;

[System.Serializable]
public sealed class TileDataReference : BaseReference<TileData, TileDataVariable>
{
    public TileDataReference() : base() { }
    public TileDataReference(TileData value) : base(value) { }
}