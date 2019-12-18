using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises a Unity event upon specified trigger events
/// </summary>
public class TriggerEvent : MonoBehaviour
{
    [SerializeField, EnumFlags]
    private Events events = 0;
    [SerializeField]
    private TriggerUnityEvent callback = new TriggerUnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (events.HasFlag(Events.OnTriggerEnter))
            callback.Invoke(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (events.HasFlag(Events.OnTriggerStay))
            callback.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (events.HasFlag(Events.OnTriggerExit))
            callback.Invoke(collision);
    }

    [System.Flags]
    private enum Events
    {
        OnTriggerEnter  = 0b0001,
        OnTriggerStay   = 0b0010,
        OnTriggerExit   = 0b0100,
    }
    [System.Serializable]
    private class TriggerUnityEvent : UnityEvent<Collider2D> { }
}
