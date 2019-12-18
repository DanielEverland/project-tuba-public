using UnityEngine;

public class DamageOnTouch : MonoBehaviour, IAbilityObject, IAbilityObjectSpawner
{
    [SerializeField]
    private Entity owner = null;
    [SerializeField]
    private AbilityAction action = null;
    [SerializeField]
    private LayerMask ignorelayer = default;
    [SerializeField]
    private FloatReference radius = new FloatReference(0.5f);
    [SerializeField]
    private BoolReference destroyOnHit = new BoolReference(false);
    [SerializeField]
    private GameObject destroyTarget;

    GameObject IAbilityObject.GameObject => gameObject;
    
    // Base radius that's scaled. The offset a bit to avoid it being exactly on the top of the collider, which would make collision inconsistent
    protected float Radius => radius.Value * ((transform.lossyScale.x + transform.lossyScale.y) / 2) + 0.01f;
    protected float Multiplier { get; set; } = 1;
    
    protected virtual void Update()
    {
        int hitCount = Physics2D.CircleCastNonAlloc(transform.position, Radius, Vector2.zero, Utility.HitBuffer, 0, ~ignorelayer);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D currentHit = Utility.HitBuffer[i];
            
            if (IsValidObject(currentHit.collider.gameObject))
                Hit(currentHit.collider.gameObject);
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsValidObject(collision.collider.gameObject))
            Hit(collision.collider.gameObject);
    }
    protected virtual void Hit(GameObject targetObject)
    {
        if (targetObject.TryGetEntity(out Entity targetEntity))
        {
            action.Tick(new AbilityData(owner, targetEntity, Multiplier));
            Destroy();
        }
        else
        {
            Destroy();
        }
    }
    protected virtual bool IsValidObject(GameObject target)
    {
        return ignorelayer != (ignorelayer | 1 << target.layer);
    }
    protected virtual void Destroy()
    {
        if (destroyOnHit.Value && destroyTarget != null)
            Destroy(destroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (destroyTarget == null)
            destroyTarget = gameObject;

        if (gameObject.TryGetEntity(out Entity entity))
            owner = entity;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
    public IAbilityObject Spawn(Ability ability, float multiplier)
    {
        DamageOnTouch damageOnTouch = GameObject.Instantiate(this);
        damageOnTouch.action = ability.Action;
        damageOnTouch.Multiplier = multiplier;

        return damageOnTouch;
    }
}