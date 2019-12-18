using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;

public abstract class AbilityAttacker : AttackerBase
{
    [SerializeField]
    private Entity owner = default;
    [SerializeField]
    private Ability ability = null;
    [SerializeField]
    private AIAttackerExitCondition exitCondition = AIAttackerExitCondition.FireOnce;
    [SerializeField]
    private UnityEvent onUseAbility = new UnityEvent();

    public Ability Ability => ability;

    protected bool ShouldReload => Ability.CurrentAmmo <= 0;
    protected bool ShouldFire => Ability.CurrentAmmo > 0;

    private bool exitConditionHasBeenFullfilled = false;
    private Status abilityStatus = Status.Running;

    protected abstract AbilityStateDelta UseAbility();

    protected virtual void Awake()
    {
        ability = Ability.Instantiate(Ability, owner);
    }

    protected override void DoStart()
    {
        exitConditionHasBeenFullfilled = false;
        abilityStatus = Status.Running;
    }
    protected override void DoStop()
    {
        Ability.OnStop();
    }
    protected override Status UpdateState()
    {
        Status updateStatus = abilityStatus;

        switch (updateStatus)
        {
            case Status.Failure:
                OnFailure();
                break;
            case Status.Success:
                OnSuccess();
                break;
        }

        return updateStatus;
    }
    protected override void DoUpdate()
    {
        PollStatus();
    }
    private void PollStatus()
    {
        if (ShouldReload && !Ability.Reloader.IsReloading)
        {
            Ability.Reloader.StartReload();

            abilityStatus = Status.Running;
        }
        else if (Ability.Reloader.IsReloading)
        {
            Ability.Reloader.DoReload();

            abilityStatus = Status.Running;
        }
        else if (CanAttack())
        {
            abilityStatus = Attack();
        }
        else
        {
            abilityStatus = Status.Failure;
        }
    }
    protected virtual Status Attack()
    {
        if (Target == null)
            return Status.Failure;

        onUseAbility.Invoke();

        AbilityStateDelta delta = UseAbility();
        ProcessExitCondition(delta);

        if (Ability.Activator.HasPersistentObjects)
        {
            return Status.Running;
        }
        else if (exitConditionHasBeenFullfilled)
        {
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }

    protected virtual void OnSuccess() { }
    protected virtual void OnFailure() { }
    protected virtual bool CanAttack() => true;

    protected AbilityInput GetDefaultInput()
    {
        return ability.Behaviour.GetInputToFire();
    }
    protected AbilityInput GetDefaultInput(Vector2 targetPosition)
    {
        AbilityInput input = GetDefaultInput();
        input.TargetPosition = targetPosition;

        return input;
    }
    protected void ProcessExitCondition(AbilityStateDelta delta)
    {
        switch (exitCondition)
        {
            case AIAttackerExitCondition.FireOnce:
                ProcessFireOnceCondition(delta);
                break;
            case AIAttackerExitCondition.AmmoEmpty:
                ProcessAmmoEmptyCondition();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    protected void ProcessFireOnceCondition(AbilityStateDelta delta)
    {
        if (delta.HasFlag(AbilityStateDelta.Fired))
            exitConditionHasBeenFullfilled = true;
    }
    protected void ProcessAmmoEmptyCondition()
    {
        if (Ability.CurrentAmmo == 0)
            exitConditionHasBeenFullfilled = true;
    }
    private void OnValidate()
    {
        owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}
