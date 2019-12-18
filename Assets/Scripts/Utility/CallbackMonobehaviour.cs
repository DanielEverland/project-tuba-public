using UnityEngine;

public abstract class CallbackMonobehaviour : MonoBehaviour, IEventHandler
{
    [SerializeField, EnumFlags]
    private CallbackState callbackState = default;

    public abstract void OnRaised();

    public void Raise()
    {
        OnRaised();
    }
    private void Awake()
    {
        if (callbackState.HasFlag(CallbackState.Awake))
            OnRaised();
    }
    private void Start()
    {
        if (callbackState.HasFlag(CallbackState.Start))
            OnRaised();
    }
    private void Update()
    {
        if (callbackState.HasFlag(CallbackState.Update))
            OnRaised();
    }
}