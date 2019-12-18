using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;

public class SelfDestructionExecutor : AttackerBase, ISelfDestructExecutor
{
    [SerializeField]
    private Entity owner = default;
    [SerializeField]
    private LayerMask layerMask = default;
    [SerializeField]
    private float radius = 5;
    [SerializeField]
    private bool dealDamage = true;
    [SerializeField]
    private AbilityAction action = default;
    [SerializeField]
    private bool destroy = true;
    [SerializeField]
    private bool applyForce = true;
    [SerializeField]
    private float force = 50;
    [SerializeField]
    private UnityEvent onSelfDestruct = new UnityEvent();

    protected override Status UpdateState()
    {
        SelfDestruct();
        return Status.Success;
    }
    public void SelfDestruct()
    {
        if(dealDamage)
            DealDamage();

        if (destroy)
            DestroyEntity();

        if (applyForce)
            ApplyForce();

        onSelfDestruct.Invoke();

        KillEntity();
    }
    private void DealDamage()
    {
        foreach (Entity entity in Utility.GetAllEntitiesWithinRadius(transform.position, radius, layerMask))
        {
            action.Tick(new AbilityData(owner, entity, 1));
        }
    }
    private void KillEntity()
    {
        owner.Health.Die();
    }
    private void DestroyEntity()
    {
        Destroy(owner.gameObject);
    }
    private void ApplyForce()
    {
        Utility.AddForceAll(transform.position, radius, force, layerMask: layerMask);
    }

    private void OnValidate()
    {
        if (owner == null)
            owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}
