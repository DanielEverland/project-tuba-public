using System;
using UnityEngine;

[System.Serializable]
public class CirclePattern : PatternComponent
{
    [SerializeField]
    private Vector2Reference offset = new Vector2Reference(Vector2.zero);
    [SerializeField]
    private FloatReference radius = new FloatReference(5);
    [SerializeField]
    private IntReference count = new IntReference(10);
    
    protected const float Radians = 2 * Mathf.PI;

    protected override void DoSpawnChildren(Action<Vector2> onSpawnChild)
    {
        for (int i = 0; i < count.Value; i++)
        {
            float angle = (Radians / count.Value) * i;
            Vector2 position = new Vector2()
            {
                x = offset.Value.x + radius.Value * Mathf.Cos(angle),
                y = offset.Value.y + radius.Value * Mathf.Sin(angle),
            };

            onSpawnChild(position);
        }
    }
}