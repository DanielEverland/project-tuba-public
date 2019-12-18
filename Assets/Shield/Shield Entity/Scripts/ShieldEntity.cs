using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will report back to the <see cref="ShieldEntitySpawner"/> when it's been killed
/// </summary>
public class ShieldEntity : MonoBehaviour
{
    private ShieldEntitySpawner owner;

    public void Initialize(ShieldEntitySpawner owner, Entity thisEntity)
    {
        this.owner = owner;
        thisEntity.Health.AddOnDeathListener(() => { owner.ElementWasDestroyed(this); });
    }
}
