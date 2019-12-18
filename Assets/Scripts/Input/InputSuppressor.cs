using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSuppressor : MonoBehaviour
{
    public static bool IsSuppressed => Time.frameCount - LastSuppressedFrame < FrameMargin;
    private static int LastSuppressedFrame { get; set; } = -FrameMargin;

    private const int FrameMargin = 3;

    [SerializeField, EnumFlags]
    private SuppressionTypes suppressionTypes = SuppressionTypes.OnUpdate;

    private RectTransform rectTransform => (RectTransform)transform;
    
    public void Update()
    {
        if (suppressionTypes.HasFlag(SuppressionTypes.OnUpdate))
            Supress();

        PollContainsMouse();
    }
    private void PollContainsMouse()
    {
        if(suppressionTypes.HasFlag(SuppressionTypes.OnHover) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
        {
            Supress();
        }
    }
    private void Supress()
    {
        LastSuppressedFrame = Time.frameCount;
    }

    [System.Flags]
    private enum SuppressionTypes
    {
        OnUpdate    = 0b0001,
        OnHover     = 0b0010,
    }
}
