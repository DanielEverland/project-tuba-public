using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "AbilityGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Custom/Ability",
    order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 0)]
public sealed class AbilityGameEvent : GameEventBase<Ability>
{
}