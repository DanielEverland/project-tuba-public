using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Overwrites the current room with a new instance
    /// </summary>
    public class RoomBuilderNewButton : MonoBehaviour
    {
        [SerializeField]
        private RoomDataVariable currentRoom = default;
        [SerializeField]
        private GameEvent rebuildRoomEvent = default;

        public void New()
        {
            currentRoom.Value = ScriptableObject.CreateInstance<RoomData>();
            rebuildRoomEvent.Raise();
        }
    }
}