using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the ability to add something to the level
/// </summary>
public interface ILevelEntity
{
    void AddToLevel(LevelBuildData levelData);
}
