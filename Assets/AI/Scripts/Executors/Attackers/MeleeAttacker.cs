using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;
using Pixelplacement;

public class MeleeAttacker : AttackerBase
{
    [SerializeField]
    private Entity owner = default;
    [SerializeField]
    private AbilityAction action = default;    
    [SerializeField]
    private Transform animationTarget = default;
    [SerializeField]
    private float moveToTime = 0.1f;
    [SerializeField]
    private float moveBackTime = 0.2f;
    
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
            case AnimationState.Done:
                return Status.Success;
            default:
                return Status.Running;
        }
    }
    protected override void DoStop()
    {
        animationState = AnimationState.Ready;
    }
    private void StartAnimation()
    {
        animationState = AnimationState.Playing;

        Play(TargetPosition);
    }
    public void Play(Vector2 targetPosition)
    {
        Tween.Position(animationTarget, TargetPosition, moveToTime, 0, Tween.EaseOutStrong, completeCallback: DealDamage);
        Tween.LocalPosition(animationTarget, Vector3.zero, moveBackTime, moveToTime, Tween.EaseOutStrong, completeCallback: FinishedAnimating);
    }
    private void FinishedAnimating()
    {
        animationState = AnimationState.Done;
    }
    private void DealDamage()
    {
        AbilityData data = new AbilityData()
        {
            Multiplier = 1,
            Owner = owner,
            Target = Target.Entity,
        };

        action.Tick(data);
    }
    private void OnValidate()
    {
        owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }

    [ContextMenu("Test Animation")]
    private void TestPlay()
    {
        float angle = Random.Range(0, 360);

        Play((Vector2)animationTarget.transform.position + angle.GetDirection() * 1);
    }
    private enum AnimationState
    {
        Ready = 0,
        Playing = 1,
        Done = 2,
    }
}
