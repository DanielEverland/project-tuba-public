using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the charge effects for <see cref="ChargedBehaviour"/>
/// </summary>
public class ChargedBehaviourEffects
{
    public ChargedBehaviourEffects(IEnumerable<AbilityRenderer> chargeEffectPrefabs, Entity owner)
    {
        Owner = owner;

        instances = new List<AbilityRenderer>();
        foreach (AbilityRenderer chargePrefab in chargeEffectPrefabs)
        {
            AbilityRenderer newRenderer = HierarchyManager.Instantiate(chargePrefab, HierarchyCategory.Effects);
            owner.AddOwnedObject(newRenderer.gameObject);
            
            instances.Add(newRenderer);
        }
    }

    private readonly Entity Owner;
    private List<AbilityRenderer> instances;

    public void UpdateColor(Color color)
    {
        foreach (AbilityRenderer renderer in instances)
        {
            renderer.UpdateColor(color);
        }
    }
    public void StartChargeEffects()
    {
        instances.ForEach(x => x.OnStarted());
    }
    public void UpdateChargeEffects(Vector2 targetPosition)
    {
        foreach (AbilityRenderer renderer in instances)
        {
            renderer.transform.position = Owner.transform.position;
            renderer.UpdatePosition(targetPosition);
        }
    }
    public void EndChargeEffect()
    {
        instances.ForEach(x => x.OnEnded());
    }
}
