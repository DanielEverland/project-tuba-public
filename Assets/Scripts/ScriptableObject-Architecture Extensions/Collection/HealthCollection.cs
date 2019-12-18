using UnityEngine;

[CreateAssetMenu(
    fileName = "HealthCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Health",
    order = 120)]
public class HealthCollection : Collection<Health>
{
}