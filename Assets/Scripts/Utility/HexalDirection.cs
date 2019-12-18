using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains pre-defined directions for Axial neighbors
/// </summary>
public struct AxialDirection
{
    public AxialDirection(int x, int y, byte bitmask)
    {
        this.x = x;
        this.y = y;
        this.bitmask = bitmask;
        Magnitude = Utility.AxialToWorldPosition(new Axial(x, y)).magnitude;
    }
    
    public static readonly AxialDirection UpRight    = new AxialDirection(1, 1,     0b010000);
    public static readonly AxialDirection UpLeft     = new AxialDirection(0, 1,     0b100000);
    public static readonly AxialDirection Right      = new AxialDirection(1, 0,     0b001000);
    public static readonly AxialDirection Left       = new AxialDirection(-1, 0,    0b000001);
    public static readonly AxialDirection DownRight  = new AxialDirection(0, -1,    0b000100);
    public static readonly AxialDirection DownLeft   = new AxialDirection(-1, -1,   0b000010);

    public static readonly AxialDirection[] AllDirections = new AxialDirection[6]
    {
        UpRight,
        UpLeft,
        Right,
        Left,
        DownRight,
        DownLeft,
    };

    /// <summary>
    /// World space magnitude
    /// </summary>
    public float Magnitude { get; }

    private readonly int x;
    private readonly int y;
    private readonly byte bitmask;

    /// <summary>
    /// Converts a vector to its nearest direction
    /// </summary>
    public static AxialDirection Convert(Vector2 vector)
    {
        (float dot, AxialDirection direction) bestDirection = (-1, UpRight);

        foreach (AxialDirection direction in AllDirections)
        {
            Vector2 worldPositionDirection = Utility.AxialToWorldPosition(direction).normalized;
            float dot = Vector2.Dot(worldPositionDirection, vector);

            if (dot > bestDirection.dot)
                bestDirection = (dot, direction);
        }

        return bestDirection.direction;
    }

    public static bool ContainsDirection(int bitmask, int mask)
    {
        return (bitmask & mask) == mask;
    }
    public static implicit operator AxialDirection(Axial axial)
    {
        return new Axial(axial.x, axial.y);
    }
    public static implicit operator Axial(AxialDirection direction)
    {
        return new Axial(direction.x, direction.y);
    }
    public static implicit operator byte(AxialDirection direction)
    {
        return direction.bitmask;
    }
    public static Axial operator +(Axial axial, AxialDirection direction)
    {
        return new Axial(axial.x + direction.x, axial.y + direction.y);
    }
    public static Axial operator -(Axial axial, AxialDirection direction)
    {
        return new Axial(axial.x - direction.x, axial.y - direction.y);
    }
    public static int operator |(AxialDirection directionA, AxialDirection directionB)
    {
        return directionA.bitmask | directionB.bitmask;
    }
    public static int operator &(AxialDirection directionA, AxialDirection directionB)
    {
        return directionA.bitmask & directionB.bitmask;
    }
}
