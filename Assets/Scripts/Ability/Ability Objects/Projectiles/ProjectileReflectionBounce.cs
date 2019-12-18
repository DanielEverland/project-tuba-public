using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bounces off environment
/// </summary>
public class ProjectileReflectionBounce : ProjectileBase
{
    [SerializeField]
    private IntReference maxBounceCount = new IntReference(10);
    [SerializeField]
    private LayerMask environmentLayer = 1 << 13 | 1 << 10;
    
    protected int BounceCount { get; set; }

    protected override void CollisionOccured(RaycastHit2D hit)
    {
        if(BounceCount >= maxBounceCount.Value)
        {
            base.CollisionOccured(hit);
        }
        else
        {
            int hitLayer = hit.collider.gameObject.layer;
            
            // We hit environment, time to bounce
            if (environmentLayer.value == (environmentLayer.value | 1 << hitLayer))
            {
                Vector2 direction = GetDirection();
                Vector2 reflection = Vector2.Reflect(direction, -hit.normal);
                
                float angle = Mathf.Atan2(reflection.y, reflection.x) * Mathf.Rad2Deg;

                transform.eulerAngles = RotationAnchor * angle;

                BounceCount++;
            }
            else
            {
                base.CollisionOccured(hit);
            }
        }
    }
}
