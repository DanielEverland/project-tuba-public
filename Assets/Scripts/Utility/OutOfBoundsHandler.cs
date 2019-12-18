using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsHandler : MonoBehaviour
{
    private Vector2? lastValidPosition = null;

    private void Update()
    {
        PollOutOfBounds();
    }
    private void FixedUpdate()
    {
        PollOutOfBounds();
    }
    private void PollOutOfBounds()
    {
        if (PathHandler.IsOutOfBounds(transform.position))
        {
            FixOutOfBounds();
        }
        else
        {
            CacheValidPosition();
        }
    }
    private void FixOutOfBounds()
    {
        if (lastValidPosition.HasValue)
        {
            Debug.LogError($"{gameObject.name} is out of bounds, but no valid position has been cached. Destroying");

            Destroy(gameObject);
        }
        else
        {
            if(Application.isEditor)
                Debug.LogWarning($"{gameObject.name} is out of bounds, fixing.");

            transform.position = lastValidPosition.Value;
        }        
    }
    private void CacheValidPosition()
    {
        if (PathHandler.IsOutOfBounds(transform.position))
            throw new System.InvalidOperationException($"Attempted to cache {transform.position}, but it is out of bounds!");

        lastValidPosition = transform.position;
    }
}
