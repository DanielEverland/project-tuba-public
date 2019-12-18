using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "UpgradeGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Upgrade",
    order = 120)]
public sealed class UpgradeGameEvent : GameEventBase<UpgradeObject>
{
}