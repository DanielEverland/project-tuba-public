using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "HealthGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Health",
    order = 120)]
public sealed class HealthGameEvent : GameEventBase<Health>
{
}