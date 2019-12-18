using UnityEngine;

[CreateAssetMenu(
    fileName = "RoomDataCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Room Data",
    order = 120)]
public class RoomDataCollection : Collection<RoomData>
{
}