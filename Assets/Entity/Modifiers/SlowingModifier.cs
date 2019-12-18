using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will slow entities movement
/// </summary>
[CreateAssetMenu(fileName = "SlowingModifier", menuName = RootMenu + "Slowing", order = Order + 3)]
public class SlowingModifier : EntityModifier
{
    public float Multiplier
    {
        get
        {
            return multiplier;
        }
        set
        {
            multiplier = value;
        }
    }

    [SerializeField, Range(0, 1)]
    private float multiplier = 0.5f;
}
