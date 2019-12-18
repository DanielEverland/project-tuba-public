using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for building the game-world structures of a level
/// </summary>
public class LevelBuilderUtility
{
    public static List<List<Vector2>> CreateCollisionEdges(Level level, IEnumerable<KeyValuePair<Axial, TileData>> tiles)
    {
        Dictionary<Vector2, List<Line>> allLines = new Dictionary<Vector2, List<Line>>();
        
        foreach (var pair in tiles)
        {
            Vector2 worldPosition = Utility.AxialToWorldPosition(pair.Key);
            byte bitmask = Utility.GetBitmask(pair.Key, level.TilePositions);
            List<Line> lines = pair.Value.Brush.GetCollisionCoordinates(bitmask);

            foreach (Line line in lines)
            {
                // A bit of a hack, but it works ¯\_(ツ)_/¯
                Vector2 scaledA = (worldPosition + (Vector2)Utility.ScaleToHexagonalSize(line.A)).RoundToNearest(0.01f);
                Vector2 scaledB = (worldPosition + (Vector2)Utility.ScaleToHexagonalSize(line.B)).RoundToNearest(0.01f);

                AddToAllLines(scaledA, new Line(scaledA, scaledB));
            }
        }

        List<List<Vector2>> positionsToReturn = new List<List<Vector2>>();
        while (allLines.Count > 0)
        {
            List<Vector2> positionsToInclude = new List<Vector2>();
            Queue<Line> linesToEvaluate = new Queue<Line>();
            Vector2 startPosition =  allLines.ElementAt(0).Key;

            // We specifically only do this twice so that our chain can move in two directions.
            for (int i = 0; i < 2; i++)
            {
                if (allLines.ContainsKey(startPosition))
                    linesToEvaluate.Enqueue(GetLine(startPosition));
            }

            while (linesToEvaluate.Count > 0)
            {
                Line line = linesToEvaluate.Dequeue();
                positionsToInclude.Add(line.A);

                if (allLines.ContainsKey(line.B))
                    linesToEvaluate.Enqueue(GetLine(line.B));
                else // We do this to close the circuit. If this is left out you'll see a bunch of missing edges
                    positionsToInclude.Add(line.B);
            }
            
            positionsToReturn.Add(positionsToInclude);
        }

        return positionsToReturn;

        Line GetLine(Vector2 index)
        {
            Line line = allLines[index][0];
            allLines[index].Remove(line);

            if (allLines[index].Count == 0)
                allLines.Remove(index);

            return line;
        }
        void AddToAllLines(Vector2 key, Line value)
        {
            if (!allLines.ContainsKey(key))
            {
                allLines.Add(key, new List<Line>());
            }

            allLines[key].Add(value);
        }
    }
}
