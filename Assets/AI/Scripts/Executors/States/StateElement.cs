using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;

public class StateElement : MonoBehaviour
{
    [SerializeField]
    private StateObject state = default;
    [SerializeField]
    private bool disableOnAwake = true;
    [SerializeField]
    private UnityEvent onEnable = new UnityEvent();
    [SerializeField]
    private UnityEvent onDisable = new UnityEvent();

    private bool isActive = false;

    private void Awake()
    {
        if(disableOnAwake)
            Disable();
    }
    public void Toggle(StateObject activeState)
    {
        if(ShouldEnable(activeState))
        {
            Enable();
        }
        else if(ShouldDisable(activeState))
        {
            Disable();
        }
    }
    private void Enable()
    {
        gameObject.SetActive(true);

        onEnable.Invoke();
    }
    private void Disable()
    {
        gameObject.SetActive(false);

        onDisable.Invoke();
    }

    private bool ShouldEnable(StateObject activeState)
    {
        return !isActive && this.state == activeState;
    }
    private bool ShouldDisable(StateObject activeState)
    {
        return isActive && this.state != activeState;   
    }
}
