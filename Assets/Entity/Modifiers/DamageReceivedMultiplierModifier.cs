using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reduces incoming damage
/// </summary>
[CreateAssetMenu(fileName = "DamageReceivedMultiplier", menuName = RootMenu + "Damage Received Multiplier", order = Order + 1)]
public class DamageReceivedMultiplierModifier : EntityModifier
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

    [SerializeField, Range(0, 10)]
    private float multiplier = 0.5f;
}
