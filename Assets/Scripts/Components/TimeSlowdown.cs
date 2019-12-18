using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    [SerializeField]
    private FloatReference duration = null;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Constant(0, 1, 1);
    
    protected float SlowdownStart { get; set; }

    public virtual void Slowdown()
    {
        SlowdownStart = Time.unscaledTime;
    }
    protected virtual void Update()
    {
        float timeSinceSlowdown = Time.unscaledTime - SlowdownStart;

        if (timeSinceSlowdown < duration.Value)
        {
            float durationPercentage = timeSinceSlowdown / duration.Value;

            Time.timeScale = curve.Evaluate(durationPercentage);
        }
    }
}