using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default wall brush
/// </summary>
public class DefaultWall : BrushBase
{
    public override BrushIdentifiers UniqueIdentifier => BrushIdentifiers.DefaultWall;
    public override string Name => "Default Wall";
    public override string[] SubmeshNames => new string[2] { "Wall", "Ceiling" };
    
    public override TileMeshData GetMeshData(byte bitmask)
    {
        return GetDefaultWalls();
    }
    private TileMeshData GetDefaultWalls()
    {
        return new TileMeshData()
        {
            Vertices = new Vector3[14]
            {
                // Walls.
                new Vector3(-0.5f,  -0.25f,     0),
                new Vector3(-0.5f,  -0.25f,     1),
                new Vector3(0,      -0.5f,      1),
                new Vector3(0,      -0.5f,      0),

                new Vector3(0,      -0.5f,      0),
                new Vector3(0,      -0.5f,      1),
                new Vector3(0.5f,   -0.25f,     1),
                new Vector3(0.5f,   -0.25f,     0),

                // Ceiling.
                new Vector3(-0.5f,  0.25f,      1),
                new Vector3(0,      0.5f,       1),
                new Vector3(0.5f,   0.25f,      1),

                new Vector3(-0.5f,  -0.25f,     1),
                new Vector3(0,      -0.5f,      1),
                new Vector3(0.5f,   -0.25f,     1),
            },

            Triangles = new int[][]
            {
                // Walls.
                new int[12]
                {
                    0, 1, 2,
                    2, 3, 0,

                    4, 5, 6,
                    6, 7, 4,
                },

                // Ceilings.
                new int[12]
                {
                    8, 9, 10,
                    8, 10, 11,

                    13, 11, 10,
                    13, 12, 11,
                }
            },

            UVs = new Vector2[14]
            {
                // Walls.
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),

                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),

                // Ceilings.
                new Vector2(Utility.UVHexelEdgeMultiplier, 0.75f),
                new Vector2(0.5f, 1),
                new Vector2(1 - Utility.UVHexelEdgeMultiplier, 0.75f),

                new Vector2(Utility.UVHexelEdgeMultiplier, 0.25f),
                new Vector2(0.5f, 0),
                new Vector2(1 - Utility.UVHexelEdgeMultiplier, 0.25f),
            },
        };
    }
    public override List<Line> GetEdgeCoordinates(byte bitmask) => DefaultEdgeCoordinates;
    public override List<Line> GetCollisionCoordinates(byte bitmask)
    {
        return new List<Line>()
        {
            new Line(-0.5f, 0.25f, 0, 0.5f),
            new Line(0, 0.5f, 0.5f, 0.25f),
            new Line(0.5f, 0.25f, 0.5f, -0.25f),
            new Line(0.5f, -0.25f, 0, -0.5f),
            new Line(0, -0.5f, -0.5f, -0.25f),
            new Line(-0.5f, -0.25f, -0.5f, 0.25f),
        };
    }
}
