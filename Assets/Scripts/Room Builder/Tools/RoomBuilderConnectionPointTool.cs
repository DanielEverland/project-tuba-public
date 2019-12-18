using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Tool responsible for assigning connection points to room
    /// </summary>
    public class RoomBuilderConnectionPointTool : RoomBuilderToolBase
    {
        protected override void DoLeftClick(Axial position, RoomData room)
        {
            if (!room.ConnectionPoints.Contains(position))
                room.ConnectionPoints.Add(position);
        }
        protected override void DoRightClick(Axial position, RoomData room)
        {
            if (room.ConnectionPoints.Contains(position))
                room.ConnectionPoints.Remove(position);
        }
    }
}
