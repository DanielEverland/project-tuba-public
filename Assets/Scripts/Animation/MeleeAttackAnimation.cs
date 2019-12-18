using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class MeleeAttackAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform animationTarget = default;
    [SerializeField]
    private float moveToTime = 0.1f;
    [SerializeField]
    private float moveBackTime = 0.2f;
    [SerializeField]
    private float moveDistance = 1;
    
    public void Play(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)animationTarget.transform.position).normalized;
        Vector2 movePosition = (Vector2)animationTarget.transform.position + direction * moveDistance;

        Tween.Position(animationTarget, movePosition, moveToTime, 0, Tween.EaseOutStrong);
        Tween.LocalPosition(animationTarget, Vector3.zero, moveBackTime, moveToTime, Tween.EaseOutStrong);
    }
    [ContextMenu("Test Animation")]
    private void TestPlay()
    {
        float angle = Random.Range(0, 360);

        Play((Vector2)animationTarget.transform.position + angle.GetDirection() * moveDistance);
    }
}
