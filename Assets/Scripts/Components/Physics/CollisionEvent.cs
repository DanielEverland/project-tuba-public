using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises a Unity event upon specified collision events
/// </summary>
public class CollisionEvent : MonoBehaviour
{
    [SerializeField, EnumFlags]
    private Events events = 0;
    [SerializeField]
    private CollisionUnityEvent callback = new CollisionUnityEvent();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (events.HasFlag(Events.OnCollisionEnter))
            callback.Invoke(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (events.HasFlag(Events.OnCollisionStay))
            callback.Invoke(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (events.HasFlag(Events.OnCollisionExit))
            callback.Invoke(collision);
    }

    [System.Flags]
    private enum Events
    {
        OnCollisionEnter    = 0b0001,
        OnCollisionStay     = 0b0010,
        OnCollisionExit     = 0b0100,
    }
    [System.Serializable]
    private class CollisionUnityEvent : UnityEvent<Collision2D> { }
}
