using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a vector field
/// </summary>
public static class VectorFieldPathfinding
{
    private static float MaxBuildInterval => 1 / MaxUpdatesPerSecond;    
    private const float MaxUpdatesPerSecond = 1;

    private static Dictionary<GameObject, VectorField> fields = new Dictionary<GameObject, VectorField>();

    public static Vector2 GetNextPosition(Vector2 currentPosition, GameObject target)
    {
        Axial targetAxial = Utility.WorldToAxialPosition(target.transform.position);
        Axial currentAxial = Utility.WorldToAxialPosition(currentPosition);

        if(TargetHasField(target))
        {
            if(FieldShouldBeRebuilt(target, targetAxial))
            {
                BuildFieldForTarget(target, targetAxial);
            }
        }
        else
        {
            BuildFieldForTarget(target, targetAxial);
        }

        return fields[target].GetTargetPosition(currentAxial);
    }
        
    private static VectorField BuildVectorField(Axial center)
    {
        Dictionary<Axial, Vector2> field = CreateNewField(center);
        Dictionary<Axial, Node> closed = CreateClosedCollection(center);
        Queue<Axial> open = CreateOpenCollection();

        EnqueueNeighbors(center, closed, open);

        Axial current = default;

        try
        {
            while (open.Count > 0)
            {
                current = open.Dequeue();

                PollPosition(current, open, closed, field);
            }

            return new VectorField(center, field);
        }
        catch (System.Exception)
        {
            HandleException(current, closed);

            throw;
        }
    }
    private static void PollPosition(Axial current, Queue<Axial> open, Dictionary<Axial, Node> closed, Dictionary<Axial, Vector2> field)
    {
        NodePosition target = GetTargetAtPosition(current, closed);

        ClosePoint(current, target.Position, closed, field);
        EnqueueNeighbors(current, closed, open);
    }
    private static NodePosition GetTargetAtPosition(Axial current, Dictionary<Axial, Node> closed)
    {
        NodePosition? bestNeighbor = null;

        for (int i = 0; i < AxialDirection.AllDirections.Length; i++)
        {
            Axial neighborPosition = current + AxialDirection.AllDirections[i];

            if (closed.ContainsKey(neighborPosition))
            {
                Node neighborEntry = closed[neighborPosition];

                if (bestNeighbor == null)
                    SetTarget();

                if (neighborEntry.Target.Steps < bestNeighbor.Value.Steps)
                    SetTarget();

                void SetTarget() => bestNeighbor = new NodePosition(neighborPosition, neighborEntry.Target.Steps);
            }
        }

        if (bestNeighbor == null)
            throw new System.NullReferenceException($"Couldn't locate a neighbor at {current}");

        return bestNeighbor.Value;
    }

    private static bool TargetHasField(GameObject target)
    {
        return fields.ContainsKey(target);
    }
    private static bool FieldShouldBeRebuilt(GameObject target, Axial targetPosition)
    {
        if (!TargetHasField(target))
            throw new System.NullReferenceException($"{target} doesn't have a field!");

        return fields[target].ShouldUpdate(targetPosition);
    }
    private static void BuildFieldForTarget(GameObject target, Axial center)
    {
        VectorField newField = BuildVectorField(center);

        fields.Overwrite(target, newField);
    }
    private static void ClosePoint(Axial point, Axial target, Dictionary<Axial, Node> closed, Dictionary<Axial, Vector2> field)
    {
        AddToField(point, target, field);
        closed.Add(point, CreateEntry(target, closed));
    }
    private static void AddToField(Axial point, Axial target, Dictionary<Axial, Vector2> field)
    {
        field.Add(point, Utility.AxialToWorldPosition(target));
    }
    private static void EnqueueNeighbors(Axial center, Dictionary<Axial, Node> closed, Queue<Axial> open)
    {
        for (int i = 0; i < AxialDirection.AllDirections.Length; i++)
        {
            Axial neighbor = center + AxialDirection.AllDirections[i];

            if (IsValidPoint(neighbor) && !closed.ContainsKey(neighbor) && !open.Contains(neighbor))
                open.Enqueue(neighbor);
        }
    }
    private static void HandleException(Axial current, Dictionary<Axial, Node> closed)
    {
#if UNITY_EDITOR
        HandleExceptionInEditor(current, closed);
#else
        Debug.LogError("Exception thrown when building vector field");
#endif
    }
    private static Node CreateEntry(Axial target, Dictionary<Axial, Node> closed)
    {
        return new Node(target, closed[target].Target.Steps + 1);
    }
    private static bool IsValidPoint(Axial point)
    {
        return Utility.IsAxialPositionWalkable(point);
    }
    private static Dictionary<Axial, Vector2> CreateNewField(Axial center)
    {
        return new Dictionary<Axial, Vector2>()
        {
            { center, Vector2.zero },
        };
    }
    private static Queue<Axial> CreateOpenCollection()
    {
        return new Queue<Axial>();
    }
    private static Dictionary<Axial, Node> CreateClosedCollection(Axial center)
    {
        return new Dictionary<Axial, Node>()
        {
            { center, new Node(center, 0) }
        };
    }

    private class VectorField
    {
        public VectorField(Axial center, Dictionary<Axial, Vector2> field)
        {
            creationTime = Time.time;

            this.center = center;
            this.field = field;
        }

        private readonly float creationTime;
        private readonly Axial center;
        private readonly Dictionary<Axial, Vector2> field;

        public Vector2 GetTargetPosition(Axial coordinate)
        {
            if (!field.ContainsKey(coordinate))
            {
#if UNITY_EDITOR
                DebugVectorField(coordinate, field);
#endif

                if (!IsValidPoint(coordinate))
                {
                    throw new System.ArgumentException($"{coordinate} is out of bounds!");
                }
                else
                {
                    throw new System.NullReferenceException($"No vector for {coordinate}");
                }                
            }
                

            return field[coordinate];
        }
        public bool ShouldUpdate(Axial currentPosition)
        {
            if(Time.time - creationTime >= MaxBuildInterval)
            {
                return center != currentPosition;
            }

            return false;
        }
    }
    private class Node
    {
        public Node(Axial target, int steps)
        {
            Target = new NodePosition()
            {
                Position = target,
                Steps = steps,
            };
        }

        public NodePosition Target { get; private set; }
    }
    private struct NodePosition
    {
        public NodePosition(Axial position, int steps)
        {
            Position = position;
            Steps = steps;
        }

        public Axial Position { get; set; }
        public int Steps { get; set; }
    }

    #region Editor Debugging
#if UNITY_EDITOR
    private const float DirectionLength = 0.25f;

    private static void HandleExceptionInEditor(Axial current, Dictionary<Axial, Node> closed)
    {
        DumpDebugDataOnException(current, closed);
        UnityEditor.EditorApplication.isPaused = true;
        
        Debug.LogError("Exception thrown when building vector field. Debugging data");
    }
    private static void DebugVectorField(Axial errorCoordinate, Dictionary<Axial, Vector2> field)
    {
        Debug.DrawLine(Vector3.zero, Utility.AxialToWorldPosition(errorCoordinate), Color.red, 5, false);

        DrawField(field);
    }
    private static void DrawField(Dictionary<Axial, Vector2> field)
    {
        foreach (var pair in field)
        {
            DrawVector(pair.Key, pair.Value);
        }
    }
    private static void DumpDebugDataOnException(Axial current, Dictionary<Axial, Node> closed)
    {
        DrawAllClosedVectors(closed);
        DrawNeighborsOfCurrentPosition(current);
    }
    private static void DrawAllClosedVectors(Dictionary<Axial, Node> closed)
    {
        foreach (var pair in closed)
        {
            DrawVector(pair.Key, pair.Value.Target.Position);
        }
    }
    private static void DrawNeighborsOfCurrentPosition(Axial current)
    {
        Vector2 currentWorldPos = Utility.AxialToWorldPosition(current);
        for (int i = 0; i < AxialDirection.AllDirections.Length; i++)
        {
            Vector2 end = currentWorldPos + Utility.AxialToWorldPosition(AxialDirection.AllDirections[i]);

            Debug.DrawLine(currentWorldPos, end, Color.red, 5);
        }
    }
    private static void DrawVector(Axial start, Vector2 end)
    {
        Vector2 startWorldPos = Utility.AxialToWorldPosition(start);
        Vector2 delta = end - startWorldPos;
        Vector2 direction = delta.normalized;
        float orthographicScale = Utility.ScaleToOrthographicVector(direction).magnitude;

        Debug.DrawRay(startWorldPos, direction * (DirectionLength * orthographicScale), Color.cyan, 5, false);
    }
#endif
    #endregion
}
