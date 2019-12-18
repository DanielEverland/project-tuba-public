using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all shapes
/// </summary>
public abstract class BrushBase
{
    public abstract BrushIdentifiers UniqueIdentifier { get; }
    public abstract string Name { get; }

    /// <summary>
    /// Names of the submeshes required for this brush.
    /// <see cref="GetTriangles(int)"/> list count must match the length of this property
    /// The names are used in-editor so the designer knows which material should be assigned to which submesh
    /// </summary>
    public abstract string[] SubmeshNames { get; }

    public abstract TileMeshData GetMeshData(byte bitmask);
    public abstract List<Line> GetEdgeCoordinates(byte bitmask);
    public abstract List<Line> GetCollisionCoordinates(byte bitmask);

    protected static List<Line> DefaultEdgeCoordinates { get; } = new List<Line>()
    {
        new Line(new Vector2(-0.5f, -0.25f), new Vector2(0, -0.5f)),
        new Line(new Vector2(0, -0.5f),      new Vector2(0.5f, -0.25f)),
    };

    protected static TileMeshData DefaultFloor { get; } = new TileMeshData()
    {
        Vertices = new Vector3[6]
        {
            new Vector2(-0.5f,  0.25f),
            new Vector2(0,      0.5f),
            new Vector2(0.5f,   0.25f),

            new Vector2(-0.5f,  -0.25f),
            new Vector2(0,      -0.5f),
            new Vector2(0.5f,   -0.25f),
        },

        Triangles = new int[][]
        {
            new int[12]
            {
                0, 1, 2,
                0, 2, 3,
                5, 3, 2,
                5, 4, 3,
            }
        },

        UVs = new Vector2[6]
        {
            new Vector2(Utility.UVHexelEdgeMultiplier, 0.75f),
            new Vector2(0.5f, 1),
            new Vector2(1 - Utility.UVHexelEdgeMultiplier, 0.75f),

            new Vector2(Utility.UVHexelEdgeMultiplier, 0.25f),
            new Vector2(0.5f, 0),
            new Vector2(1 - Utility.UVHexelEdgeMultiplier, 0.25f),
        },
    };
}