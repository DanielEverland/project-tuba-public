using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the coordinates of an edge on a brush
/// </summary>
[System.Serializable]
public struct Line
{
    public Line(float ax, float ay, float bx, float by)
    {
        A = new Vector2(ax, ay);
        B = new Vector2(bx, by);
    }
    public Line(Vector2 a, Vector2 b)
    {
        A = a;
        B = b;
    }

    public Vector2 A;
    public Vector2 B;
}
