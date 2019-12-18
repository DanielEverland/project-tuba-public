using UnityEngine;

[System.Serializable]
public sealed class RoomDataReference : BaseReference<RoomData, RoomDataVariable>
{
    public RoomDataReference() : base() { }
    public RoomDataReference(RoomData value) : base(value) { }
}