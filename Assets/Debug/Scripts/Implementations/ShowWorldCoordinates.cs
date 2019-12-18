using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Shows world coordinates on screen
/// </summary>
[CreateAssetMenu(fileName = "ShowWorldCoordinates.asset", menuName = MenuRoot + "Show World Coordinates", order = MenuOrder)]
public class ShowWorldCoordinates : DebugElementToggle
{
    [SerializeField]
    private TMP_Text textElementPrefab = default;

    private TMP_Text textInstance;
    
    public override void OnDebugElementDisable()
    {
        GameObject.Destroy(textInstance.gameObject);
    }
    public override void OnDebugElementEnable()
    {
        textInstance = CreateTextElement();
    }
    public override void OnDebugElementUpdate()
    {
        Vector2 worldPosition = Utility.MousePositionInWorld;

        if (Input.GetKey(KeyCode.LeftAlt))
            worldPosition = Utility.RoundToNearestHexagonalPosition(worldPosition);
        else if (Input.GetKey(KeyCode.LeftShift))
            worldPosition = worldPosition.RoundToNearest(1);
        
        Vector2 screenSpacePosition = Camera.main.WorldToScreenPoint(worldPosition);
        

        textInstance.transform.position = screenSpacePosition;
        textInstance.text = worldPosition.ToString("F2");
    }
    private TMP_Text CreateTextElement()
    {
        TMP_Text indicatorElement = GameObject.Instantiate(textElementPrefab);
        indicatorElement.transform.SetParent(Root);

        return indicatorElement;
    }
}
