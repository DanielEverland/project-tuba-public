using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventWrapper : CallbackMonobehaviour
{
    [SerializeField]
    private UnityEvent onRaised = default;

    public override void OnRaised()
    {
        onRaised.Invoke();
    }
}
