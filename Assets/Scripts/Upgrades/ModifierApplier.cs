using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a given <see cref="UpgradeModifier"/>
/// </summary>
public abstract class ModifierApplier : MonoBehaviour
{
    private void OnEnable()
    {
        Upgrades.AddListener(PollModifier);
    }
    private void OnDisable()
    {
        Upgrades.RemoveListener(PollModifier);
    }

    protected abstract void PollModifier();
}
