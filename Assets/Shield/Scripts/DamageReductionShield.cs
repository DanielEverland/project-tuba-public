using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A damage reduction shield will decrease incoming damage
/// </summary>
public class DamageReductionShield : Shield
{
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);

    private DamageReceivedMultiplierModifier modifier;
    
    protected override void Awake()
    {
        base.Awake();

        modifier = ScriptableObject.CreateInstance<DamageReceivedMultiplierModifier>();
        entity.AddModifier(modifier);
    }
    protected override void UpdateModifiers()
    {
        float percentage = Mathf.Clamp01((float)CurrentLayers / maxLayers.Value);
        modifier.Multiplier = curve.Evaluate(percentage);
        entity.SetModifierAsDirty(modifier);
    }
}
