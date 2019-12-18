using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleThenDestroy : CallbackMonobehaviour
{
    [SerializeField]
    private GameObject target = default;
    [SerializeField]
    private Vector3 targetScale = Vector3.zero;
    [SerializeField]
    private float duration = 1;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public override void OnRaised()
    {
        Tween.LocalScale(
            target: target.transform,
            startValue: transform.localScale,
            endValue: targetScale,
            easeCurve: curve,
            duration: duration,
            delay: delay,
            completeCallback: () => Destroy(target));
    }
    private void OnValidate()
    {
        if (target == null)
        {
            if (gameObject.TryGetEntity(out Entity entity))
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
