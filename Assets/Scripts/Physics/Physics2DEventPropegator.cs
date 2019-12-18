using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Propegates events created by a <see cref="Collider2D"/>
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Physics2DEventPropegator : MonoBehaviour
{
    [SerializeField]
    private Collision2DEvent onCollisionEnter = new Collision2DEvent();
    [SerializeField]
    private Collision2DEvent onCollisionStay = new Collision2DEvent();
    [SerializeField]
    private Collision2DEvent onCollisionExit = new Collision2DEvent();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter.Invoke(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        onCollisionStay.Invoke(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onCollisionExit.Invoke(collision);
    }

    [System.Serializable]
    private class Collision2DEvent : UnityEvent<Collision2D> { }
}
