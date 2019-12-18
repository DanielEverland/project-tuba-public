using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the behaviour of an entity
/// </summary>
public abstract class EntityModifier : ScriptableObject
{
    protected const string RootMenu = Utility.MenuItemRoot + "Entity Modifiers/";
    protected const int Order = 100;
}
