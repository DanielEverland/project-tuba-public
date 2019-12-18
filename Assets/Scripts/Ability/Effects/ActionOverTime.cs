using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies an action over time
/// </summary>
public class ActionOverTime : MonoBehaviour
{
    public static void Apply(AbilityAction.Component component, AbilityData data)
    {
        data.Target.gameObject.AddComponent<ActionOverTime>().Initialize(component, data);
    }

    private AbilityData data = default;
    private AbilityAction.Component action = default;
    private float startTime = default;
    private float timeSinceLastTick = default;
    private float ticksCompleted = default;

    private float TickInterval => action.EffectOverTimeDuration / action.TotalTicks;

    public void Initialize(AbilityAction.Component component, AbilityData data)
    {
        action = component;
        this.data = data;
        this.data.Multiplier /= action.TotalTicks;

        startTime = Time.time;
    }
    private void Update()
    {
        timeSinceLastTick += Time.deltaTime;

        if(timeSinceLastTick >= TickInterval)
        {
            timeSinceLastTick -= TickInterval;
            ticksCompleted++;

            action.Tick(data);
        }

        if(ticksCompleted >= action.TotalTicks)
        {
            Destroy(this);
        }
    }
}
