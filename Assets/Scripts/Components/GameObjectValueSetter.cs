using UnityEngine;

public class GameObjectValueSetter : CallbackMonobehaviour
{
    [SerializeField]
    private GameObjectVariable target = null;
    [SerializeField]
    private GameObject value = null;

    public override void OnRaised() => target.Value = value;
}