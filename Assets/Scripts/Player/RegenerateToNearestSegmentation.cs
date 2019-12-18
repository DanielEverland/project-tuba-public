using UnityEngine;

public class RegenerateToNearestSegmentation : MonoBehaviour
{
    [SerializeField]
    protected FloatReference currentValue = null;
    [SerializeField]
    private FloatReference valuePerSegmentation = null;
    [SerializeField]
    private GameEvent onRegenerate = null;
    
    public void Regenerate()
    {
        float targetValue = Utility.GetMaxValueForSegment(currentValue.Value, valuePerSegmentation.Value);

        // If we're already at segment max, we want to get to the next segment max
        if (targetValue == currentValue.Value)
            targetValue += valuePerSegmentation.Value;

        currentValue.Value = targetValue;
        onRegenerate?.Raise();
    }
}