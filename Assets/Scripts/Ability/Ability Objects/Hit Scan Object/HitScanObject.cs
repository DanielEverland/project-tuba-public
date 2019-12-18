using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of projectile that doesn't use physics but instantly checks whether it hits anything
/// </summary>
[RequireComponent(typeof(AbilityRendererHandler))]
public class HitScanObject : MonoBehaviour, IAbilityObject, IAbilityObjectSpawner, ISetTargetDirection
{
    [SerializeField, HideInInspector]
    private AbilityRendererHandler rendererHandler;
    [SerializeField]
    private LayerMask ignoreLayer = default;
    [SerializeField]
    private FloatReference animationTime = new FloatReference(0.3f);
    [SerializeField]
    private FloatReference maxDistance = new FloatReference(50);
    [SerializeField]
    private AnimationCurve animationTimeCurve = AnimationCurve.EaseInOut(0, 0, 0.15f, 1);

    public GameObject GameObject => gameObject;
    
    protected Vector2 Direction { get; private set; }
    protected Ability Ability { get; private set; }
    protected float Multiplier { get; private set; }
    protected float TimeSinceFire { get; private set; }
    protected Vector2 AnimationTargetPoint { get; private set; }

    private bool shouldFire = false;

    private void Update()
    {
        if(shouldFire)
        {
            shouldFire = false;
            TimeSinceFire = 0;

            Fire();
        }

        TimeSinceFire += Time.deltaTime;
        Animate();
    }
    protected virtual void Fire()
    {
        rendererHandler.Start();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, maxDistance.Value, ~ignoreLayer);

        if (hit)
        {
            if (hit.collider.gameObject.TryGetEntity(out Entity entity))
            {
                Ability.Action.Tick(new AbilityData(Ability.Owner, entity, Multiplier));
            }

            AnimationTargetPoint = hit.point;
        }
        else
        {
            AnimationTargetPoint = (Vector2)transform.position + Direction * maxDistance.Value;
        }
    }
    protected virtual void Animate()
    {
        float percentage = Mathf.Clamp01(TimeSinceFire / animationTime.Value);
        float animatedTime = animationTimeCurve.Evaluate(percentage);
        Vector2 interpolatedPosition = Vector2.Lerp(transform.position, AnimationTargetPoint, animatedTime);
        rendererHandler.UpdatePosition(interpolatedPosition);

        if (TimeSinceFire >= animationTime.Value)
            Destroy();
    }
    protected virtual void Destroy()
    {
        rendererHandler.End();
        Destroy(gameObject);
    }
    public void SetTargetDirection(Vector2 direction)
    {
        Direction = direction;
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        HitScanObject instance = Instantiate(this);
        instance.Ability = ability;
        instance.shouldFire = true;
        instance.Multiplier = multiplier;
        instance.transform.position = ability.Owner.transform.position;

        return instance;
    }
    private void OnValidate()
    {
        rendererHandler = GetComponent<AbilityRendererHandler>();
    }
}
