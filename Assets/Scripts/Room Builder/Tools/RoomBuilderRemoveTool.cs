using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Tool responsible for removing tiles in a room
    /// </summary>
    public class RoomBuilderRemoveTool : RoomBuilderToolBase
    {
        protected override void DoLeftClick(Axial position, RoomData room)
        {
            room.Tiles.Remove(position);
        }
    }
}