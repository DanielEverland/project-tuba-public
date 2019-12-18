using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileExplosion : ProjectileBase
{
    [SerializeField]
    private ExplosionObject explosionObject = default;

    protected override void DoDamage(RaycastHit2D hit)
    {
        IAbilityObject obj = explosionObject.Spawn(Ability, Multiplier);

        if(obj is ISetTargetPosition positionSetter)
            positionSetter.SetTargetPosition(transform.position);

        Destroy(gameObject);
    }
}
