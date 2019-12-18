using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileStraightLinePenetration : ProjectileExplosion
{
    [SerializeField]
    private float maxDistance = 5;
    
    protected override void Update()
    {
        Move();
        PollExplode();
    }
    protected virtual void PollExplode()
    {
        float distanceTravelled = Vector2.Distance(transform.position, StartPosition);

        if (distanceTravelled >= maxDistance)
        {
            DoDamage(default);
        }
    }
}
