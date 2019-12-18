using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    /// <summary>
    /// Defines which room builder tool is currently in use
    /// </summary>
    [CreateAssetMenu(fileName = "ToolState.asset", menuName = Utility.MenuItemRoomBuilder + "Tool State", order = 400)]
    public class RoomBuilderToolState : ScriptableObject
    {
    }
}