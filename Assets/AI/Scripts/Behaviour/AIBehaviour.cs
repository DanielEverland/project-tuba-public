using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls how often a given AI component should update
/// </summary>
public abstract class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    protected Entity owner;
    [SerializeField]
    private AIUpdateInterval updateInterval = default;

    private float timeSinceLastTick = 0;
    
    protected virtual void Update()
    {
        PollTick();
    }
    private void PollTick()
    {
        while(timeSinceLastTick >= updateInterval)
        {
            Tick();

            timeSinceLastTick -= updateInterval;
        }

        timeSinceLastTick += Time.deltaTime;
    }

    protected abstract void Tick();

    private void OnValidate()
    {
        owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}
