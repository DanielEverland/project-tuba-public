using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

/// <summary>
/// Offsets a <see cref="GameObject"/> by a <see cref="Vector3"/> and then destroys it
/// </summary>
public class OffsetThenDestroy : CallbackMonobehaviour
{
    [SerializeField]
    private GameObject target = default;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private float duration = 1;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    public override void OnRaised()
    {
        Tween.Position(
            target: target.transform,
            startValue: transform.position,
            endValue: transform.position + offset,
            easeCurve: curve,
            duration: duration,
            delay: delay,
            completeCallback: () => Destroy(target));
    }
    private void OnValidate()
    {
        if(target == null)
        {
            if(gameObject.TryGetEntity(out Entity entity))
            {
                target = entity.gameObject;
            }
            else
            {
                target = gameObject;
            }
        }
    }
}
