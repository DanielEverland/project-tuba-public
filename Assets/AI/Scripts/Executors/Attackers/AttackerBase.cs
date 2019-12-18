using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackerBase : MonoBehaviour, IAttacker
{
    protected Interactable Target { get; private set; }
    protected Vector2 TargetPosition => Target.transform.position;

    protected bool IsActive { get; private set; }

    public void OnStart(Interactable target)
    {
        if (target == null)
            throw new System.NullReferenceException($"<b>{nameof(AttackerBase)}</b>: Received null target!");

        IsActive = true;

        Target = target;
        DoStart();
    }
    public Status OnUpdate()
    {
        return UpdateState();
    }
    public void OnStop()
    {
        IsActive = false;

        DoStop();
    }

    protected virtual void Update()
    {
        if (IsActive)
            DoUpdate();
    }

    protected abstract Status UpdateState();
    protected virtual void DoUpdate() { }
    protected virtual void DoStart() { }
    protected virtual void DoStop() { }
}