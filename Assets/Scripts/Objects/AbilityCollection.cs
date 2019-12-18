using UnityEngine;

[CreateAssetMenu(
    fileName = "AbilityCollection.asset",
    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "Ability",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 0)]
public class AbilityCollection : Collection<Ability>
{
}