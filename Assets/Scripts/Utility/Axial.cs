using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position in an axial-coordinate system
/// </summary>
[System.Serializable]
public struct Axial
{
    public Axial(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;
    
    public static Axial zero => new Axial(0, 0);

    public static Axial FromWorldPosition(Vector2 worldPosition)
    {
        return Utility.WorldToAxialPosition(worldPosition);
    }
    public static implicit operator Vector2(Axial axial)
    {
        return new Vector2(axial.x, axial.y);
    }
    public static explicit operator Axial(Vector2 vector)
    {
        return new Axial((int)vector.x, (int)vector.y);
    }
    public static bool operator !=(Axial a, Axial b)
    {
        return a.x != b.x || a.y != b.y;
    }
    public static bool operator ==(Axial a, Axial b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static Axial operator +(Axial a, Vector2 b)
    {
        return new Axial(a.x + (int)b.x, a.y + (int)b.y);
    }
    public static Axial operator -(Axial a, Vector2 b)
    {
        return new Axial(a.x - (int)b.x, a.y - (int)b.y);
    }
    public override bool Equals(object obj)
    {
        if(obj is Axial axial)
        {
            return axial == this;
        }

        return false;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int i = 13;

            i += x.GetHashCode() * 17;
            i += y.GetHashCode() * 7;

            return i;
        }
    }
    public override string ToString()
    {
        return $"[{x}, {y}]";
    }
}
