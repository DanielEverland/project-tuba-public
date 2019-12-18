using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for handling a path instance
/// </summary>
public interface IPath
{
    Vector2 TargetPosition { get; }
    Vector2 GetNextPosition(Vector2 currentPosition);
}
