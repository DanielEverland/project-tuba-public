using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    [System.Serializable]
    [CreateAssetMenu(
    fileName = "RoomBuilderToolStateGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Room Builder/Tool State",
    order = 120)]
    public sealed class RoomBuilderToolStateGameEvent : GameEventBase<RoomBuilderToolState>
    {
    }
}