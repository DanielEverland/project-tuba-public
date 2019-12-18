using UnityEngine;

namespace EverlandGames.RoomBuilder
{
    [CreateAssetMenu(
    fileName = "RoomBuilderToolStateCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Room Builder/Tool State",
    order = 120)]
    public class RoomBuilderToolStateCollection : Collection<RoomBuilderToolState>
    {
    }
}