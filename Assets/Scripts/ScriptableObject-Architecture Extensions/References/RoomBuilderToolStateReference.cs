using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    [System.Serializable]
    public sealed class RoomBuilderToolStateReference : BaseReference<RoomBuilderToolState, RoomBuilderToolStateVariable>
    {
        public RoomBuilderToolStateReference() : base() { }
        public RoomBuilderToolStateReference(RoomBuilderToolState value) : base(value) { }
    }
}