using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds the edges of a tile
/// </summary>
public static class EdgeBuilder
{
    private static readonly float EdgeDepth = 0.25f;

    public static void Build(LevelEnvironmentData data, TileData tile, Vector3 worldPosition, byte bitmask)
    {
        foreach (Line edge in tile.Brush.GetEdgeCoordinates(bitmask))
        {
            int vertexCount = data.Vertices.Count;

            // Vertices.
            Vector3 scaledA = Utility.ScaleToHexagonalSize(edge.A) + worldPosition;
            Vector3 scaledB = Utility.ScaleToHexagonalSize(edge.B) + worldPosition;

            data.Vertices.AddRange(new Vector3[4]
            {
                scaledA,
                scaledB,
                scaledB + new Vector3(0, 0, EdgeDepth),
                scaledA + new Vector3(0, 0, EdgeDepth),
            });

            // Triangles.
            data.SetTriangles(tile.EdgeMaterial, new int[6]
            {
                vertexCount + 0, vertexCount + 1, vertexCount + 2,
                vertexCount + 2, vertexCount + 3, vertexCount + 0,
            });

            // UVs.
            data.UVs.AddRange(new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            });
        }
    }
}
