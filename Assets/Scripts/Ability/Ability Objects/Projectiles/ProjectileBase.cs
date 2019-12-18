using System.Linq;
using UnityEngine;

public class ProjectileBase : ObjectMover, IAbilityObjectSpawner, IAbilityObject, ISetTargetDirection
{
    [SerializeField]
    private LayerMask ignoreLayer = (int)Layer.Enemy;
    [SerializeField]
    private RaycastType raycastType = RaycastType.Line;
    [SerializeField]
    private FloatReference circleCastRadius = new FloatReference(0.5f);
    [SerializeField]
    private FloatReference distance = new FloatReference(10);

    public GameObject GameObject => gameObject;

    protected static readonly Vector3 RotationAnchor = new Vector3(0, 0, 1);
    protected const int PermanentIgnoreLayer = (int)Layer.EnvironmentNoBlock;

    protected LayerMask AllIgnoreLayers => ignoreLayer | PermanentIgnoreLayer;
    protected float Multiplier { get; private set; } = 1;
    protected Ability Ability { get; private set; }
    protected Vector2 StartPosition { get; set; }
    
    public virtual void Initialize(Ability ability, float multiplier = 1)
    {
        Ability = ability;
        Multiplier = multiplier;
    }
    protected virtual void Start()
    {
        StartPosition = transform.position;
    }
    protected override void Update()
    {
        base.Update();

        PollCollision();
        PollLifetimeEnded();
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        ProjectileBase instance = Instantiate(this);
        instance.Initialize(ability, Multiplier);

        return instance;
    }
    public void SetTargetDirection(Vector2 direction)
    {
        transform.eulerAngles = RotationAnchor * direction.GetAngle();
    }
    protected virtual void PollLifetimeEnded()
    {
        if(Vector2.Distance(StartPosition, transform.position) >= distance.Value)
            Destroy();
    }
    protected virtual void PollCollision()
    {
        Vector3 direction = GetDirection();
        int hitCount = DoHitScan(direction.normalized);

        if(hitCount > 0)
        {
            CollisionOccured(Utility.HitBuffer[0]);
        }        
    }
    protected virtual int DoHitScan(Vector2 direction)
    {
        switch (raycastType)
        {
            case RaycastType.Line:
                return Physics2D.RaycastNonAlloc(transform.position, direction, Utility.HitBuffer, velocity.Value * Time.deltaTime, ~AllIgnoreLayers);
            case RaycastType.Sphere:
                return Physics2D.CircleCastNonAlloc(transform.position, GetRadius(), direction, Utility.HitBuffer, velocity.Value * Time.deltaTime, ~AllIgnoreLayers);
            default:
                throw new System.NotImplementedException();
        }
    }
    protected virtual void CollisionOccured(RaycastHit2D hit)
    {
        DoDamage(hit);
        Destroy();
    }
    protected virtual void DoDamage(RaycastHit2D hit)
    {
        if (hit.collider.gameObject.TryGetEntity(out Entity entity))
            Tick(entity);
    }
    protected virtual float GetRadius()
    {
        // We want to scale radius. There's no good way around this, it's safe to assume circular objects will have uniform scaling
        return circleCastRadius.Value * transform.localScale.x;
    }
    protected virtual void Tick(Entity target)
    {
        if(target != null)
            Ability.Action.Tick(new AbilityData(Ability.Owner, target, Multiplier));
    }
    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }

    private enum RaycastType
    {
        Line = 0,
        Sphere = 1,
    }
}