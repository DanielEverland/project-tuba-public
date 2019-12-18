using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTargetSeeking : ProjectileBase
{
    [SerializeField]
    private EntityCollection potentialTargets = default;
    [SerializeField]
    private float torque = 1;
    
    protected Entity Target { get; set; }

    protected override void Update()
    {
        PollTarget();
        RotateTowardsTarget();

        base.Update();
    }
    protected virtual void RotateTowardsTarget()
    {
        if (Target == null)
            return;

        Vector2 direction = (Target.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, torque * Time.deltaTime);

        transform.eulerAngles = RotationAnchor * newAngle;
    }
    protected virtual void PollTarget()
    {
        float? bestDot = null;
        Entity bestTarget = null;
        foreach (Entity target in potentialTargets)
        {
            Vector2 normalizedDirection = (target.transform.position - transform.position).normalized;
            float dot = Vector2.Dot(normalizedDirection, transform.right);

            if(BetterCandidate(dot))
            {
                bestDot = dot;
                bestTarget = target;
            }
        }

        if (bestTarget != null && bestTarget != Target)
            AssignTarget(bestTarget);

        bool BetterCandidate(float dot)
        {
            if (bestDot.HasValue)
            {
                return bestDot.Value < dot;
            }

            return true;
        }
    }
    protected virtual void AssignTarget(Entity target)
    {
        Target = target;

        Debug.DrawLine(transform.position, target.transform.position, Color.cyan, 1);
    }
}
