using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Tool responsible for adding tiles to a room
    /// </summary>
    public class RoomBuilderAddTool : RoomBuilderToolBase
    {
        [SerializeField]
        private TileDataVariable currentTile = default;

        protected override void DoLeftClick(Axial position, RoomData room)
        {
            if(currentTile.Value != null)
                room.Tiles.Overwrite(position, currentTile.Value);
        }
    }
}