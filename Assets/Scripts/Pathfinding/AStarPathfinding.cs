using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

/// <summary>
/// Calculates A* pathfinding
/// </summary>
public static class AStarPathfinding
{
    private static Dictionary<AxialDirection, float> distance = new Dictionary<AxialDirection, float>();
    
    public static AStarPath GetPath(Vector2 start, Vector2 end)
    {
        return new AStarPath(start, end);
    }
    public static LinkedList<Vector2> GetRawPath(Vector2 start, Vector2 end, bool addToDebug = false)
    {
        PathfindingResult result = RunPathfinder(Utility.WorldToAxialPosition(start), Utility.WorldToAxialPosition(end));

        if(addToDebug)
            SendToDebugging(result);

        return result.FinishedPath;
    }
    private static PathfindingResult RunPathfinder(Axial start, Axial end)
    {
        SimplePriorityQueue<Node> openNodes = new SimplePriorityQueue<Node>();
        Dictionary<Axial, Node> closedNodes = new Dictionary<Axial, Node>();
        
        Node startNode = new Node(start, 0, GetHeuristic(start, end));
        Node current = null;
        openNodes.Enqueue(startNode, startNode.Cost);

        while (openNodes.Count > 0)
        {
            current = openNodes.Dequeue();
            closedNodes.Add(current.Position, current);

            if (current == end) // Reached end
                break;
            
            foreach (AxialDirection direction in AxialDirection.AllDirections)
            {
                Axial neighborPosition = current.Position + direction;

                if (!Utility.IsAxialPositionWalkable(neighborPosition))
                    continue;
                
                float newNeighborCost = current.Cost + GetMovementCost(current, direction);
                float heuristic = GetHeuristic(neighborPosition, end);
                Node neighbor = new Node(neighborPosition, newNeighborCost, heuristic, current);

                // Update surrounding nodes if we have a better path, even if they've already been evaluated
                if (openNodes.Contains(neighbor))
                {
                    // The heuristic is deterministic, so this will get us the pure cost of the node
                    float currentCost = openNodes.GetPriority(neighbor) - heuristic;

                    if (newNeighborCost < currentCost)
                    {
                        openNodes.Remove(neighbor);
                    }
                }
                if (closedNodes.ContainsKey(neighborPosition))
                {
                    float currentCost = closedNodes[neighborPosition].Cost;
                    if (newNeighborCost < currentCost)
                    {
                        closedNodes.Remove(neighborPosition);
                    }
                }

                // Add them to the queue if applicable
                if (!openNodes.Contains(neighbor) && !closedNodes.ContainsKey(neighbor.Position))
                {
                    openNodes.Enqueue(neighbor, neighbor.FullValue);
                }
            }
        }

        return CalculateResults(current);

        PathfindingResult CalculateResults(Node endNode)
        {
            if (endNode == null)
            {
                Debug.LogError($"Couldn't calculate path between {start} and {end}");
                return null;
            }

            PathfindingResult result = new PathfindingResult()
            {
                OpenNodes = openNodes,
                CloesdNodes = closedNodes,
            };

            result.FinishedPath = new LinkedList<Vector2>();
            result.FinishedPath.AddFirst(endNode);

            Node currentNode = endNode;
            while (currentNode.Parent != null)
            {
                currentNode = closedNodes[currentNode.Parent.Value];
                result.FinishedPath.AddFirst(currentNode);
            }

            // We're already in the first node, walking to it would just force us to walk to the center of the node, and we don't want that.
            result.FinishedPath.RemoveFirst();

            return result;
        }
    }
    private static float GetHeuristic(Axial currentPosition, Axial endPosition)
    {
        return Vector2.Distance(
            Utility.AxialToWorldPosition(currentPosition),
            Utility.AxialToWorldPosition(endPosition));
    }
    private static float GetMovementCost(Node current, AxialDirection direction)
    {
        return direction.Magnitude;
    }
    private static void SendToDebugging(PathfindingResult result)
    {
        PathfindingDebugger.AddPath(result.FinishedPath, result.OpenNodes, result.CloesdNodes);
    }
    public class Node
    {
        public Node(Axial position, float cost, float heuristic)
        {
            this.Position = position;
            this.Cost = cost;
            this.Heuristic = heuristic;
        }
        public Node(Axial position, float cost, float heuristic, Node parent)
        {
            Parent = parent;

            this.Position = position;
            this.Cost = cost;
            this.Heuristic = heuristic;
        }

        public Axial? Parent { get; set; }
        public Axial Position { get; }
        public float Cost { get; }
        public float Heuristic { get; }
        public float FullValue => Cost + Heuristic;

        public override bool Equals(object obj)
        {
            if (obj is Node node)
            {
                return node.Position == this.Position;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
        public override string ToString()
        {
            return $"{Position} - {Parent}";
        }
        public static implicit operator Axial(Node node)
        {
            return node.Position;
        }
        public static implicit operator Vector2(Node node)
        {
            return Utility.AxialToWorldPosition(node.Position);
        }
    }
    private class PathfindingResult
    {
        public LinkedList<Vector2> FinishedPath;
        public SimplePriorityQueue<Node> OpenNodes;
        public Dictionary<Axial, Node> CloesdNodes;
    }
}
