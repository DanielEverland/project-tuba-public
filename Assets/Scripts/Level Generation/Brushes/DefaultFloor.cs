using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default floor
/// </summary>
public class DefaultFloor : BrushBase
{
    public override BrushIdentifiers UniqueIdentifier => BrushIdentifiers.DefaultFloor;
    public override string Name => "Default Floor";
    public override string[] SubmeshNames => new string[1] { "Floor" };

    public override TileMeshData GetMeshData(byte bitmask) => DefaultFloor;
    public override List<Line> GetEdgeCoordinates(byte bitmask) => DefaultEdgeCoordinates;
    public override List<Line> GetCollisionCoordinates(byte bitmask)
    {
        List<Line> lines = new List<Line>();

        if((bitmask & AxialDirection.UpLeft) != AxialDirection.UpLeft)
        {
            lines.Add(new Line(-0.5f, 0.25f, 0, 0.5f));
        }
        if ((bitmask & AxialDirection.UpRight) != AxialDirection.UpRight)
        {
            lines.Add(new Line(0, 0.5f, 0.5f, 0.25f));
        }
        if ((bitmask & AxialDirection.Right) != AxialDirection.Right)
        {
            lines.Add(new Line(0.5f, 0.25f, 0.5f, -0.25f));
        }
        if ((bitmask & AxialDirection.DownRight) != AxialDirection.DownRight)
        {
            lines.Add(new Line(0.5f, -0.25f, 0, -0.5f));
        }
        if ((bitmask & AxialDirection.DownLeft) != AxialDirection.DownLeft)
        {
            lines.Add(new Line(0, -0.5f, -0.5f, -0.25f));
        }
        if ((bitmask & AxialDirection.Left) != AxialDirection.Left)
        {
            lines.Add(new Line(-0.5f, -0.25f, -0.5f, 0.25f));
        }

        return lines;
    }
}