using System.Collections.Generic;
using UnityEngine;

public class PatternMoveToPoint : PatternBehaviour
{
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private FloatReference animationTime = new FloatReference(5);
    
    public override void Update()
    {
        float time = Mathf.Clamp01((Time.time - StartTime) / animationTime.Value);
        float animationValue = curve.Evaluate(time);

        Evaluate(x =>
        {
            Pattern.SetElementPosition(x, Vector3.Lerp(x.StartingPosition, Vector3.zero, animationValue));
        });

        if (time == 1)
            Destroy(Pattern.gameObject);
    }
}