using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides an API for applying effects to entities
/// </summary>
public abstract class EntityAffecter : MonoBehaviour
{
    protected virtual void OnEntityEnter(Entity entity) { }
    protected virtual void OnEntityExit(Entity entity) { }
    protected virtual void OnEntityStay(Entity entity) { }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityEnter(entity);
        }
    } 
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityStay(entity);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityExit(entity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityEnter(entity);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityStay(entity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetEntity(out Entity entity))
        {
            OnEntityExit(entity);
        }
    }    
}
