using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows <see cref="Axial"/> coordinates on screen
/// </summary>
[CreateAssetMenu(fileName = "ShowAxialCoordinates.asset", menuName = MenuRoot + "Show Axial Coordinates", order = MenuOrder)]
public class ShowAxialCoordinates : DebugElementToggle
{
    [SerializeField]
    private TMP_Text textElement = default;

    private const int IndicatorRange = 7;

    [System.NonSerialized]
    private List<TMP_Text> allTextElements = new List<TMP_Text>();
    [System.NonSerialized]
    private Queue<TMP_Text> availableTextElements = new Queue<TMP_Text>();
    
    public override void OnDebugElementDisable()
    {
        foreach (Object obj in allTextElements)
        {
            GameObject.Destroy(obj);
        }

        allTextElements.Clear();
    }
    public override void OnDebugElementUpdate()
    {
        availableTextElements = new Queue<TMP_Text>(allTextElements);

        Vector2 mousePosInWorld = Utility.MousePositionInWorld;
        Vector2 worldPosRoundedToHexagon = Utility.RoundToNearestHexagonalPosition(mousePosInWorld);
        
        Utility.EvaluateAxialGrid(worldPosRoundedToHexagon, IndicatorRange, IndicatorRange, x =>
        {
            TMP_Text textElement = GetTextElement();

            Vector2 worldPosition = Utility.AxialToWorldPosition(x);
            textElement.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
            textElement.text = x.ToString();
        });
    }
    private TMP_Text GetTextElement()
    {
        if(availableTextElements.Count == 0)
        {
            return CreateTextElement();
        }
        else
        {
            return availableTextElements.Dequeue();
        }
    }
    private TMP_Text CreateTextElement()
    {
        TMP_Text indicatorElement = GameObject.Instantiate(textElement);
        indicatorElement.transform.SetParent(Root);

        allTextElements.Add(indicatorElement);

        return indicatorElement;
    }
}
