using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilityRendererHandler))]
public class BeamBase : MonoBehaviour, IAbilityObjectSpawner, IPersistentAbilityObject, ISetTargetDirection, ISetTargetPosition
{
    [SerializeField, HideInInspector]
    private AbilityRendererHandler rendererHandler = null;
    [SerializeField]
    private LayerMask ignoreLayer = (int)Layer.Player;
    [SerializeField]
    private FloatReference duration = new FloatReference(1);
    [SerializeField]
    private IntReference ticks = new IntReference(10);
    [SerializeField]
    private FloatReference maxDistance = new FloatReference(20);

    public GameObject GameObject => gameObject;
    
    protected Ability Ability { get; private set; }
    protected Vector2 Direction { get; private set; }
    protected Entity HitObject { get; private set; }
    protected float TickInterval => duration.Value / ticks.Value;
    protected float TimeSinceLastTick { get; set; }
    protected float TotalTime { get; set; }
    protected float MuzzleMulitplier { get; set; }

    private static readonly Color DebugColor = new Color(1, 0, 0, 0.2f);

    private void Start()
    {
        rendererHandler.Start();

        transform.SetParent(Ability.Owner.transform);
        transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        PerformRaycast();
        CalculateTicks();

        TotalTime += Time.deltaTime;
    }
    private void CalculateTicks()
    {
        TimeSinceLastTick += Time.deltaTime;

        if (TimeSinceLastTick >= TickInterval)
        {
            TimeSinceLastTick -= TickInterval;
            PerformTick();
        }
    }
    private void PerformTick()
    {
        if (HitObject != null)
        {
            AbilityData data = new AbilityData()
            {
                Owner = Ability.Owner,
                Target = HitObject,
                Multiplier = 1 / ticks.Value * MuzzleMulitplier,
            };

            Ability.Action.Tick(data);
        }

        rendererHandler.Tick();
    }
    private void PerformRaycast()
    {
        HitObject = null;
        RaycastHit2D hit = default;
        int hitCount = Physics2D.RaycastNonAlloc(transform.position, Direction, Utility.HitBuffer, maxDistance.Value, ~ignoreLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (Utility.HitBuffer[i].collider.gameObject.TryGetEntity(out Entity entity))
            {
                if (entity != Ability.Owner)
                {
                    HitObject = entity;
                }
                else
                {
                    continue;
                }
            }

            hit = Utility.HitBuffer[i];
            break;
        }

        if (hit != default)
        {
            rendererHandler.UpdatePosition(hit.point);
            Debug.DrawLine(transform.position, hit.point, DebugColor);
        }
        else
        {
            rendererHandler.UpdatePosition(transform.position + (Vector3)Direction * maxDistance.Value);
            Debug.DrawLine(transform.position, transform.position + (Vector3)Direction * maxDistance.Value, DebugColor);
        }
    }
    public void SetTargetDirection(Vector2 direction)
    {
        Direction = direction;
    }
    public void SetTargetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        BeamBase beamBase = Instantiate(this);
        beamBase.Ability = ability;
        beamBase.MuzzleMulitplier = multiplier;

        return beamBase;
    }
    public bool ShouldBeDestroyed()
    {
        if(TotalTime >= duration.Value)
        {
            Destroy();
            return true;
        }

        return false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
        rendererHandler.End();
    }
    private void OnValidate()
    {
        rendererHandler = GetComponent<AbilityRendererHandler>();
    }
}
