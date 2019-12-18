using UnityEngine;

public class HealthValueSetter : CallbackMonobehaviour
{
    [SerializeField]
    private HealthVariable target = null;
    [SerializeField]
    private Health value = null;

    public override void OnRaised() => target.Value = value;
}