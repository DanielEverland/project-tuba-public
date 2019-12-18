using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = AStarPathfinding.Node;
using TMPro;

/// <summary>
/// Text element used to display values of a <see cref="Node"/>
/// </summary>
public class PathfindingDebugTextElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text costElement = default;
    [SerializeField]
    private TMP_Text heuristicElement = default;
    [SerializeField]
    private TMP_Text fullCostElement = default;

    private Vector3 worldPosition;

    public void Initialize(Node node)
    {
        worldPosition = Utility.AxialToWorldPosition(node.Position);
        
        costElement.text = GetText(node.Cost);
        heuristicElement.text = GetText(node.Heuristic);
        fullCostElement.text = GetText(node.FullValue);
    }
    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(worldPosition);
    }
    private string GetText(float value)
    {
        return Mathf.RoundToInt(value * 10).ToString();
    }
}
