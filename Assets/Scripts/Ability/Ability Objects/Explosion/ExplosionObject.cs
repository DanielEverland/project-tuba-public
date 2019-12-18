using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ability Object that simply spawns an explosion
/// </summary>
public class ExplosionObject : MonoBehaviour, IAbilityObject, IAbilityObjectSpawner, ISetTargetPosition
{
    [SerializeField]
    private Ability ability = default;
    [SerializeField]
    private LayerMask ignoreLayer = default;
    [SerializeField]
    private FloatReference radius = new FloatReference(2);
    [SerializeField]
    private FloatReference delay = new FloatReference(0);
    [SerializeField]
    private new ParticleSystem particleSystem = default;    
    [SerializeField]
    private UnityEvent onExplosionEvent = default;

    public GameObject GameObject => gameObject;
    
    protected float Multiplier { get; set; } = 1;
    protected float StartTime { get; set; }
    protected float TimeSinceStart { get; set; }
    
    protected virtual void Update()
    {
        TimeSinceStart += Time.deltaTime;

        if(TimeSinceStart >= delay.Value)
        {
            Explode();
        }
    }
    protected virtual void Explode()
    {
        int hitCount = Utility.GetAllObjectsWithinRadius(transform.position, radius.Value, ~ignoreLayer.value, ref Utility.ColliderBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            if(Utility.ColliderBuffer[i].gameObject.TryGetEntity(out Entity entity))
            {
                ability.Action.Tick(new AbilityData(ability.Owner, entity, Multiplier));
            }
        }

        onExplosionEvent.Invoke();
        SpawnParticleSystem();
        Destroy(gameObject);
    }
    protected virtual void SpawnParticleSystem()
    {
        ParticleSystem instance = Instantiate(particleSystem);
        instance.transform.position = transform.position;
    }
    public void SetTargetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        ExplosionObject obj = HierarchyManager.Instantiate(this, HierarchyCategory.Effects);
        obj.ability = ability;
        obj.Multiplier = multiplier;
        obj.StartTime = Time.time;

        return obj;
    }
}
