using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    [CreateAssetMenu(
    fileName = "RoomBuilderToolStateVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Room Builder/Tool State",
    order = 120)]
    public class RoomBuilderToolStateVariable : BaseVariable<RoomBuilderToolState>
    {
    }
}