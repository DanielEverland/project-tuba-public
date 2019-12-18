using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Base class for roombuilder tools
    /// </summary>
    public abstract class RoomBuilderToolBase : MonoBehaviour, IRoomBuilderTool
    {
        [SerializeField]
        private RoomBuilderToolState thisTool = default;

        public RoomBuilderToolState State => thisTool;

        public void OnLeftClick(Axial position, RoomData room)
        {
            DoLeftClick(position, room);
        }
        public void OnRightClick(Axial position, RoomData room)
        {
            DoRightClick(position, room);
        }
        protected virtual void DoLeftClick(Axial position, RoomData room) { }
        protected virtual void DoRightClick(Axial position, RoomData room) { }
    }
}
