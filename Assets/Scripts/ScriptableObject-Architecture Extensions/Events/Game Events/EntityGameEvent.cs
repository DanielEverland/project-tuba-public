using UnityEngine;

[System.Serializable]
[CreateAssetMenu(
    fileName = "EntityGameEvent.asset",
    menuName = SOArchitecture_Utility.GAME_EVENT + "Entity",
    order = 120)]
public sealed class EntityGameEvent : GameEventBase<Entity>
{
}