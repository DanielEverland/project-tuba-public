using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An A* path built by <see cref="AStarPathfinding"/>
/// </summary>
public class AStarPath : IPath
{
	public AStarPath(Vector2 start, Vector2 end)
    {
        TargetPosition = end;

        this.rawPath = AStarPathfinding.GetRawPath(start, end);
    }

    private readonly LinkedList<Vector2> rawPath;

    public Vector2 TargetPosition { get; }

    public Vector2 GetNextPosition(Vector2 currentPosition)
    {
        Axial currentAxial = Utility.WorldToAxialPosition(currentPosition);
        Axial firstPathNodeAxial = Utility.WorldToAxialPosition(rawPath.First.Value);

        if (currentAxial == firstPathNodeAxial)
            rawPath.RemoveFirst();

        return rawPath.First.Value;
    }
}
