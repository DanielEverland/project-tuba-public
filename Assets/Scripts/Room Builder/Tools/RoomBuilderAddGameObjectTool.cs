using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Allows the ability to add normal prefabs to a room
    /// </summary>
    public class RoomBuilderAddGameObjectTool : RoomBuilderToolBase
    {
        protected override void DoLeftClick(Axial position, RoomData room)
        {
#if UNITY_EDITOR
            if(Selection.activeGameObject != null)
                room.Prefabs.Overwrite(position, Selection.activeGameObject);
#endif
        }
        protected override void DoRightClick(Axial position, RoomData room)
        {
            if (room.Prefabs.ContainsKey(position))
                room.Prefabs.Remove(position);
        }
    }
}
