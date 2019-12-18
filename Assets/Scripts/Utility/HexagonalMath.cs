using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains some math helper functions
/// </summary>
public static class HexagonalMath
{
    #region WorldToHexagonal
    // Based upon https://stackoverflow.com/questions/7705228/hexagonal-grids-how-do-you-find-which-hexagon-a-point-is-in

    private static readonly float QuarterHeight = Utility.HexelHeight / 4;
    private static readonly float GridHeight = QuarterHeight * 3;
    private static readonly float GridWidth = Utility.HexelWidth;
    private static readonly float HalfWidth = GridWidth / 2;

    public static Vector2 WorldToHexagonalPosition(Vector2 worldPos)    
    {
        // Offset the row so the grid starts at the top of each hexagon
        worldPos.y += QuarterHeight;

        // Find the row and column of the box that the point falls in
        float column = worldPos.x;

        Vector2 roundedPosition = new Vector2()
        {
            y = Mathf.Floor(worldPos.y / GridHeight) * GridHeight,
        };

        bool isOddRow = Mathf.RoundToInt(Mathf.Abs(roundedPosition.y / GridHeight)) % 2 == 1;

        if (isOddRow)
        {
            // Offset the grid to match with the column
            column -= HalfWidth;

            // Round to nearest whole
            roundedPosition.x = column.RoundToNearest(1);

            // Offset so it's aligned properly
            roundedPosition.x += HalfWidth;
        }
        else
        {
            roundedPosition.x = worldPos.x.RoundToNearest(1);
        }

        // Work out the position of the point relative to the box it is in
        float relativeX = worldPos.x - roundedPosition.x + HalfWidth;
        float relativeY = worldPos.y - roundedPosition.y;
        
        // Calculate which edge we're within, if any
        float slope = QuarterHeight / HalfWidth;

        // We're within the top section of the hexagon
        bool isWithinEdgeBox = relativeY > GridHeight - QuarterHeight;

        // Left side
        if (isWithinEdgeBox && relativeX < HalfWidth)
        {
            float height = slope * relativeX + (GridHeight - QuarterHeight);

            // Within top left edge
            if (relativeY > height)
            {
                roundedPosition.y += GridHeight;
                roundedPosition.x -= HalfWidth;
            }
        }
        else if (isWithinEdgeBox && relativeX > HalfWidth) // Right side
        {
            float height = -slope * relativeX + (GridHeight + QuarterHeight);

            // Within top right edge
            if (relativeY > height)
            {
                roundedPosition.y += GridHeight;
                roundedPosition.x += HalfWidth;
            }
        }

        return roundedPosition;
    }
    #endregion
}
