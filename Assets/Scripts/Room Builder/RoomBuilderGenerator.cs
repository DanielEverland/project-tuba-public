using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Creates the actual rooms in the room builder from <see cref="RoomData"/>
    /// </summary>
    public class RoomBuilderGenerator : MonoBehaviour
    {
        [SerializeField]
        private RoomDataVariable currentRoom = default;
        [SerializeField, HideInInspector]
        private GameObject previousRoom = default;

        private void Start()
        {
            Regenerate();
        }
        public void Regenerate()
        {
            if (currentRoom.Value == null)
                return;

            if (previousRoom != null)
            {
                GameObject.Destroy(previousRoom);
            }

            previousRoom = LevelBuilder.Build(currentRoom.Value).Root;
        }
    }
}