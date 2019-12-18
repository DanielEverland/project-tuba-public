using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strcture containing all necessary data for an ability to tick
/// </summary>
public struct AbilityData
{
    public AbilityData(Entity owner, Entity target, float multiplier)
    {
        Owner = owner;
        Target = target;
        Multiplier = multiplier;
    }

    public Entity Owner;
    public Entity Target;
    public float Multiplier;
}
