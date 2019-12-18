using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Interface for room builder tools
    /// </summary>
    public interface IRoomBuilderTool
    {
        RoomBuilderToolState State { get; }

        void OnLeftClick(Axial position, RoomData room);
        void OnRightClick(Axial position, RoomData room);
    }
}