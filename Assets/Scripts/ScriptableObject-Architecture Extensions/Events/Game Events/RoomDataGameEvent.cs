using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "RoomDataGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Room Data",
    order = 120)]
public sealed class RoomDataGameEvent : GameEventBase<RoomData>
{
}