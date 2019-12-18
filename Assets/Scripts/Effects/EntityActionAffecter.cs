using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies an action to entities that stand within the affecter with an optional interval
/// </summary>
public class EntityActionAffecter : EntityAffecter
{
    [SerializeField]
    private AbilityAction action = default;
    [SerializeField]
    private FloatReference tickInterval = new FloatReference(1);

    private Dictionary<Entity, float> entityStayDurations = new Dictionary<Entity, float>();

    protected override void OnEntityEnter(Entity entity)
    {
        base.OnEntityEnter(entity);

        if (!entityStayDurations.ContainsKey(entity))
        {
            entityStayDurations.Add(entity, 0);
            TickEntity(entity);
        }        
    }
    protected override void OnEntityExit(Entity entity)
    {
        base.OnEntityExit(entity);

        entityStayDurations.Remove(entity);
    }
    private void Update()
    {
        PollTicks();
    }
    private void PollTicks()
    {
        foreach (Entity entity in entityStayDurations.Keys.ToList())
        {
            if (entity == null) // Entity has been killed or destroyed in another way
            {
                entityStayDurations.Remove(entity);
                return;
            }


            float time = entityStayDurations[entity];
            time += Time.deltaTime;

            while (time > tickInterval.Value)
            {
                time -= tickInterval.Value;

                TickEntity(entity);
            }

            entityStayDurations[entity] = time;
        }
    }
    private void TickEntity(Entity entity)
    {
        AbilityData data = new AbilityData() { Target = entity, Multiplier = 1 };
        action.Tick(data);
    }
}
