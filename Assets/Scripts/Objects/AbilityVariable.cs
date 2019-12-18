using UnityEngine;

[CreateAssetMenu(
    fileName = "AbilityVariable.asset",
    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Custom/Ability",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_VARIABLES + 0)]
public sealed class AbilityVariable : BaseVariable<Ability>
{
}