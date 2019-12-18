using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slows entities on touch
/// </summary>
public class EntitySlower : EntityAffecter
{
    [SerializeField]
    private FloatReference multiplier = new FloatReference(0.5f);

    private SlowingModifier modifier;

    private void Awake()
    {
        modifier = ScriptableObject.CreateInstance<SlowingModifier>();
        modifier.Multiplier = multiplier.Value;
    }
    protected override void OnEntityEnter(Entity entity)
    {
        entity.AddModifier(modifier);
    }
    protected override void OnEntityExit(Entity entity)
    {
        entity.RemoveModifier(modifier);
    }
}
