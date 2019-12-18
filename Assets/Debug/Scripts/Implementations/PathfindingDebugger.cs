using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using Node = AStarPathfinding.Node;

/// <summary>
/// Debug element that allows you to see pathfinding data
/// </summary>
[CreateAssetMenu(fileName = "ShowPathfinding.asset", menuName = MenuRoot + "Show Pathfinding", order = MenuOrder)]
public class PathfindingDebugger : DebugElementToggle
{
    [SerializeField]
    private PathfindingDebugTextElement textElementPrefab = default;

    private const float EntryDuration = 5;

    private static List<PathEntry> entries = new List<PathEntry>();
    private static bool shouldUpdateText = false;
    private static bool isOn;

    private Vector2? debugStart;
    private Vector2? debugEnd;

    private const float GLAlpha = 0.2f;
    private static readonly Dictionary<NodeType, Color> Colors = new Dictionary<NodeType, Color>()
    {
        { NodeType.Path, new Color(0, 1, 1, GLAlpha) },
        { NodeType.Open, new Color(0, 1, 0, GLAlpha) },
        { NodeType.Closed, new Color(1, 0, 0, GLAlpha) },
    };

    private List<PathfindingDebugTextElement> instantiatedElements = default;
    private Queue<PathfindingDebugTextElement> availableElements = default;
    private Transform uiParent = default;
    
    public static void AddPath(LinkedList<Vector2> path, SimplePriorityQueue<Node> openPositions, Dictionary<Axial, Node> closedPositions)
    {
        if (!isOn)
            return;

        PathEntry entry = new PathEntry();

        foreach (Vector2 position in path)
        {
            Axial axialPosition = Axial.FromWorldPosition(position);

            Node node = closedPositions[axialPosition];
            entry.Nodes.Add(axialPosition, (node, NodeType.Path));
        }
        foreach (Node node in openPositions)
        {
            if(!entry.Nodes.ContainsKey(node.Position))
            {
                entry.Nodes.Add(node.Position, (node, NodeType.Open));
            }
        }
        foreach (var pair in closedPositions)
        {
            if (!entry.Nodes.ContainsKey(pair.Key))
            {
                Node node = closedPositions[pair.Key];
                entry.Nodes.Add(pair.Key, (node, NodeType.Closed));
            }
        }

        entries.Add(entry);
        shouldUpdateText = true;
    }
    public override void OnDebugElementEnable()
    {
        base.OnDebugElementEnable();

        isOn = true;
        instantiatedElements = new List<PathfindingDebugTextElement>();
        availableElements = new Queue<PathfindingDebugTextElement>();

        if (uiParent == null)
        {
            uiParent = new GameObject("Pathfinding Debugger Parent").transform;
            uiParent.SetParent(DebugInitializer.Canvas2D.transform);
        }
    }
    public override void OnDebugElementDisable()
    {
        base.OnDebugElementDisable();

        isOn = false;
        entries.Clear();
        UpdateText();
    }
    public override void OnDebugElementUpdate()
    {
        PollInput();

        for (int i = entries.Count - 1; i >= 0; i--)
        {
            if (entries[i].PollRemove())
            {
                entries.RemoveAt(i);
                shouldUpdateText = true;
            }
        }

        if (shouldUpdateText)
            UpdateText();
    }
    private void UpdateText()
    {
        shouldUpdateText = false;

        ResetElements();

        foreach (PathEntry entry in entries)
        {
            foreach (var nodeData in entry.Nodes.Values)
            {
                PathfindingDebugTextElement textElement = GetElement();
                textElement.gameObject.SetActive(true);
                textElement.Initialize(nodeData.node);
            }
        }
    }
    private void ResetElements()
    {
        availableElements.Clear();

        foreach (PathfindingDebugTextElement element in instantiatedElements)
        {            
            element.gameObject.SetActive(false);
            availableElements.Enqueue(element);
        }
    }
    private PathfindingDebugTextElement GetElement()
    {
        if(availableElements.Count > 0)
        {
            return availableElements.Dequeue();
        }
        else
        {
            return CreatNewTextElement();
        }
    }
    private PathfindingDebugTextElement CreatNewTextElement()
    {
        PathfindingDebugTextElement newInstance = GameObject.Instantiate(textElementPrefab);
        newInstance.transform.SetParent(uiParent);
        instantiatedElements.Add(newInstance);

        return newInstance;
    }
    private void PollInput()
    {
        Vector2? prevStart = debugStart;
        Vector2? prevEnd = debugEnd;
        
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                debugStart = Utility.MousePositionInWorld;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                debugEnd = Utility.MousePositionInWorld;
            }
        }

        if((prevStart != debugStart || prevEnd != debugEnd)
            &&
            (debugStart.HasValue && debugEnd.HasValue))
        {
            AStarPathfinding.GetRawPath(debugStart.Value, debugEnd.Value, true);

            debugStart = null;
            debugEnd = null;
        }
    }
    public override void OnDebugElementRenderObject()
    {
        LineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.TRIANGLES);
        
        foreach (PathEntry entry in entries)
        {
            foreach (var pair in entry.Nodes)
            {
                NodeType type = pair.Value.type;
                GL.Color(Colors[type]);

                DrawHexagon(Utility.AxialToWorldPosition(pair.Key));
            }
        }

        GL.End();
        GL.PopMatrix();
    }

    private class PathEntry
    {
        public float TimeLeft = EntryDuration;
        public Dictionary<Axial, (Node node, NodeType type)> Nodes = new Dictionary<Axial, (Node, NodeType)>();

        public bool PollRemove()
        {
            TimeLeft -= Time.deltaTime;

            return TimeLeft <= 0;
        }
    }
    private enum NodeType
    {
        None = 0,

        Path,
        Open,
        Closed,
    }
}
