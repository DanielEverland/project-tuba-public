using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ability objects are manifistations of abilities like projectiles or patterns
/// </summary>
public interface IAbilityObject
{
    GameObject GameObject { get; }
}
