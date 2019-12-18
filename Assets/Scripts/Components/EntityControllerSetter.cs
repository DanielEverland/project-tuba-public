using UnityEngine;

public class EntityControllerSetter : CallbackMonobehaviour
{
    [SerializeField]
    private EntityControllerVariable target = null;
    [SerializeField]
    private EntityController value = null;

    public override void OnRaised() => target.Value = value;
}