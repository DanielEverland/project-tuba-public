using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides instances of an <see cref="IPath"/> based on parameters given by AI
/// </summary>
public static class PathHandler
{
	public static IPath GetPath(Interactable target)
    {
        return GetPath(target.gameObject);
    }
    public static IPath GetPath(GameObject gameObject)
    {
        return new VectorFieldPath(gameObject);
    }
    public static IPath GetPath(Vector2 start, Vector2 end)
    {
        return new AStarPath(start, end);
    }
    public static bool IsOutOfBounds(Vector2 position)
    {
        Axial axialPosition = Utility.WorldToAxialPosition(position);
        return !Utility.IsAxialPositionWalkable(axialPosition);
    }

    private class VectorFieldPath : IPath
    {
        public VectorFieldPath(GameObject target)
        {
            this.target = target;
        }

        private readonly GameObject target;

        public Vector2 TargetPosition => target.transform.position;
        public Vector2 GetNextPosition(Vector2 currentPosition)
        {
            return VectorFieldPathfinding.GetNextPosition(currentPosition, target);
        }
    }
}
