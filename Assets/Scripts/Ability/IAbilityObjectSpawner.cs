using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates IAbilityObjects
/// </summary>
public interface IAbilityObjectSpawner
{
    IAbilityObject Spawn(Ability ability, float multiplier);
}
