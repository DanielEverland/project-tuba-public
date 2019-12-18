using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Handles the mouse input for room building
    /// </summary>
    public class RoomBuilderInputHandler : MonoBehaviour
    {
        [SerializeField]
        private List<IRoomBuilderTool> tools = default;
        [SerializeField]
        private RoomBuilderToolStateVariable currentTool = default;
        [SerializeField]
        private RoomDataVariable currentRoom = default;
        [SerializeField]
        private GameEvent rebuildEvent = default;

        private Vector2 oldPosition;

        private void Awake()
        {
            if (currentRoom.Value == null)
                currentRoom.Value = ScriptableObject.CreateInstance<RoomData>();
        }
        private void Update()
        {
            if (InputSuppressor.IsSuppressed || currentRoom.Value == null)
                return;
            
            Vector2 roundedWorldPosition = Utility.RoundToNearestHexagonalPosition(Utility.MousePositionInWorld);
            Axial position = Utility.WorldToAxialPosition(roundedWorldPosition);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
            {
                OnLeftClick(position);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse1))
            {
                OnRightClick(position);
            }

            oldPosition = position;
        }
        private void OnLeftClick(Axial position)
        {
            GetTool().OnLeftClick(position, currentRoom.Value);
            rebuildEvent.Raise();
        }
        private void OnRightClick(Axial position)
        {
            GetTool().OnRightClick(position, currentRoom.Value);
            rebuildEvent.Raise();
        }
        private IRoomBuilderTool GetTool()
        {
            foreach (IRoomBuilderTool tool in tools)
            {
                if (tool.State == currentTool.Value)
                    return tool;
            }

            throw new System.NotImplementedException($"No tool implementation for {currentTool.Value.name}");
        }
        private void OnValidate()
        {
            tools = new List<IRoomBuilderTool>(GetComponentsInChildren<IRoomBuilderTool>());
        }
    }
}