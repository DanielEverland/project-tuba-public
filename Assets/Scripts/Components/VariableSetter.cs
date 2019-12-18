using UnityEngine;

public class VariableSetter : CallbackMonobehaviour
{
    [SerializeField]
    private VariableSetterCondition conditions = VariableSetterCondition.None;
    [SerializeField]
    private BaseVariable target = null;
    [SerializeField]
    private BaseVariable source = null;
    
    public override void OnRaised()
    {
        if (conditions.HasFlag(VariableSetterCondition.TargetNull) && target.BaseValue != null)
            return;

        target.BaseValue = source.BaseValue;
    }
}
public abstract class VariableSetter<TVariable, TValue> : CallbackMonobehaviour where TVariable : BaseVariable<TValue>
{
    [SerializeField]
    private VariableSetterCondition conditions = VariableSetterCondition.None;
    [SerializeField]
    private TVariable target = null;
    [SerializeField]
    private TValue source = default;

    public override void OnRaised()
    {
        if (conditions.HasFlag(VariableSetterCondition.TargetNull) && target.BaseValue != null)
            return;

        target.BaseValue = source;
    }
}
public enum VariableSetterCondition
{
    None            = 0b0000,

    TargetNull      = 0b0001,
}