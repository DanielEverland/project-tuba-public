using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "ThemeGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Theme",
    order = 120)]
public sealed class ThemeGameEvent : GameEventBase<Theme>
{
}