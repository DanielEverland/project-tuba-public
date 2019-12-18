using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class AIDashAttacker : AttackerBase
{
    [SerializeField]
    private Dasher dasher = default;
    [SerializeField]
    private new Animation animation = default;
    [SerializeField]
    private FloatReference animationDuration = new FloatReference(1);
    
    private float animationStartTime = 0;
    private AnimationState animationState = AnimationState.Ready;
    
    protected override Status UpdateState()
    {
        switch (animationState)
        {
            case AnimationState.Ready:
                {
                    StartAnimation();
                    return Status.Running;
                }
            case AnimationState.Running:
                {
                    PollEndAnimation();
                    return Status.Running;
                }
            case AnimationState.Done:
                {
                    return dasher.IsDashing ? Status.Running : Status.Success;
                }
            default:
                throw new System.NotImplementedException();
        }
    }
    private void DoDash()
    {
        Vector2 direction = (TargetPosition - (Vector2)transform.position).normalized;
        
        dasher.Dash(direction);
    }
    private void StartAnimation()
    {
        animationStartTime = Time.time;
        animation.Play();

        animationState = AnimationState.Running;
    }
    private void PollEndAnimation()
    {
        if(Time.time - animationStartTime >= animationDuration.Value)
        {
            animationState = AnimationState.Done;
            animation.Stop();

            DoDash();
        }
    }
    protected override void DoStop()
    {
        animationState = AnimationState.Ready;
        animation.Stop();
    }

    private enum AnimationState
    {
        Ready = 0,
        Running = 1,
        Done = 2,
    }
}
