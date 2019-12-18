using System;
using UnityEngine;

[System.Serializable]
public class LinePattern : PatternComponent
{
    [SerializeField]
    private Vector2Reference startOffset = new Vector2Reference(new Vector2(-5, 0));
    [SerializeField]
    private Vector2Reference endOffset = new Vector2Reference(new Vector2(5, 0));
    [SerializeField]
    private IntReference count = new IntReference(6);

    protected override void DoSpawnChildren(Action<Vector2> onSpawnChild)
    {
        float totalDistance = Vector2.Distance(startOffset.Value, endOffset.Value);
        float distanceBetweenObjects = totalDistance / count.Value;

        for (int i = 0; i < count.Value; i++)
        {
            float percentage = (float)i / (count.Value - 1);
            Vector2 position = Vector2.Lerp(startOffset.Value, endOffset.Value, percentage);

            onSpawnChild(position);
        }
    }
}