using UnityEngine;
using UnityEngine.UI;

public abstract class BaseFillSetter<T> : MonoBehaviour where T : BaseVariable
{
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private T value = default;
    [SerializeField]
    private T max = default;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private bool inverse = false;

    protected T Value { get { return value; } }
    protected T Max { get { return max; } }

    private void Update()
    {
        image.fillAmount = GetFillValue();
    }
    protected virtual float GetFillValue()
    {
        if (GetValue() == 0 || GetMaxValue() == 0)
            return 0;

        float value = Mathf.Clamp(GetValue() / GetMaxValue(), 0f, 1f);
        float curveValue = curve.Evaluate(value);

        if (inverse)
        {
            return 1 - curveValue;
        }
        else
        {
            return curveValue;
        }
    }
    protected virtual float GetValue() => System.Convert.ToSingle(value.BaseValue);
    protected virtual float GetMaxValue() => System.Convert.ToSingle(max.BaseValue);
}