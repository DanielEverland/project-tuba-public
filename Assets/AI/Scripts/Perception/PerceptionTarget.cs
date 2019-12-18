using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionTarget : System.IEquatable<Interactable>
{
    public PerceptionTarget(Perception owner, Interactable target)
    {
        if (owner == null)
            throw new System.NullReferenceException($"Tried to create a {nameof(PerceptionTarget)} with a null owner");

        if (target == null)
            throw new System.NullReferenceException($"Tried to create a {nameof(PerceptionTarget)} with a null target");

        this.owner = owner;
        this.target = target;

        lastRaycastTime = Time.time;
    }

    private readonly Perception owner;
    private Interactable target;

    public Interactable Target
    {
        get
        {
            if (target == null)
                return null;

            if (CanSeeTarget())
            {
                return target;
            }
            else
            {
                return null;
            }
        }
        set
        {
            target = value;
        }
    }    

    private float SightDistance => owner.SightDistance;
    private float MaxRaycastInterval => 1 / RaycastsAllowedPerSecond;

    private const float RaycastsAllowedPerSecond = 2;

    private float lastRaycastTime = float.MinValue;
    private bool cachedCanSeeTargetResult = true;

    private bool CanSeeTarget()
    {
        if(Time.time - lastRaycastTime > MaxRaycastInterval)
        {
            cachedCanSeeTargetResult = Perception.CanSeeTarget(owner, target.transform.position);
            lastRaycastTime = Time.time;
        }

        return cachedCanSeeTargetResult;
    }
    public static implicit operator Interactable(PerceptionTarget perceptionTarget)
    {
        return perceptionTarget?.Target;
    }
    public override bool Equals(object obj)
    {
        if(obj is Interactable interactable)
        {
            return Equals(interactable);
        }

        return false;
    }
    public bool Equals(Interactable interactable)
    {
        return Target == interactable;
    }
    public override int GetHashCode()
    {
        return Target != null ? Target.GetHashCode() : -1;
    }
}
