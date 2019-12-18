using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityController : MonoBehaviour
{
    [SerializeField]
    private Entity entity = default;
    [SerializeField]
    private FloatReference drag = new FloatReference(1);
    [SerializeField]
    private FloatReference mass = new FloatReference(1);
    [SerializeField]
    private BoolReference enablePhysics = new BoolReference(true);
    [SerializeField, HideInInspector]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private bool enableDebug = false;
    
    public Vector2 MovementDirection { get; private set; }
    
    protected Vector2 Velocity { get => rigidbody.velocity; private set => rigidbody.velocity = value; }
    protected Vector2 MoveDelta { get; private set; }
    
    private const float MovementPathDuration = 5;

    private const float NudgeForce = 50;

    // No fucking idea why this is required.
    private const float MagicForceDivider = 50;

    private float speedMultiplier = 1;

    private void Awake()
    {
        entity.AddModifierListener(UpdateSpeedMultiplier, typeof(SlowingModifier));
    }
    private void UpdateSpeedMultiplier()
    {
        int modifierCount = entity.GetModifierCount(typeof(SlowingModifier));
        speedMultiplier = 1;

        for (int i = 0; i < modifierCount; i++)
        {
            speedMultiplier = Mathf.Min(speedMultiplier, entity.GetModifier<SlowingModifier>(i).Multiplier);
        }
    }
    public virtual void AddForce(Vector2 force, ForceMode2D forceMode)
    {
        if (float.IsNaN(force.x) || float.IsNaN(force.y) || !enablePhysics.Value)
            return;

        switch (forceMode)
        {
            case ForceMode2D.Force:
                Velocity += force / mass.Value / MagicForceDivider * Time.fixedDeltaTime;
                break;
            case ForceMode2D.Impulse:
                Velocity += force / mass.Value / MagicForceDivider;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    public virtual void Move(Vector2 direction, Scaling scaling = Scaling.Orthographic)
    {
        if (scaling == Scaling.Orthographic)
            direction = Utility.ScaleToOrthographicVector(direction);
        
        MoveDelta = direction * speedMultiplier * Time.fixedDeltaTime;
    }
    /// <summary>
    /// Will move the entity to <paramref name="targetPosition"/> without exceeding
    /// </summary>
    public virtual void MoveTo(Vector2 targetPosition, float movementSpeed)
    {
        Vector2 delta = (targetPosition - (Vector2)transform.position);
        Vector2 direction = delta.normalized;

        MoveDelta = direction * movementSpeed * Time.fixedDeltaTime;

        // About to exceed
        if (MoveDelta.magnitude > delta.magnitude)
            MoveDelta = delta;
    }
    protected virtual void FixedUpdate()
    {
        Vector2 targetPosition = transform.position;
        targetPosition += Utility.ScaleToOrthographicVector(Velocity) * Time.fixedDeltaTime;
        targetPosition += MoveDelta;

        if(enableDebug)
            Debug.DrawLine(transform.position, targetPosition, Color.cyan, MovementPathDuration, false);

        MovementDirection = MoveDelta.normalized;
        rigidbody.MovePosition(targetPosition);

        Velocity *= Mathf.Clamp01(1f - drag.Value * Time.fixedDeltaTime);
        MoveDelta = Vector2.zero;
    }
    protected virtual void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.hideFlags |= HideFlags.HideInInspector;
        rigidbody.gravityScale = 0;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.interpolation = RigidbodyInterpolation2D.None;
        rigidbody.bodyType = enablePhysics.Value ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;

        if(entity == null)
        {
            if (gameObject.TryGetEntity(out Entity entity))
                this.entity = entity;
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.transform.position == transform.position)
            Nudge();
    }
    /// <summary>
    /// Nudge transform a tiny bit
    /// Used to avoid having objects directly on top of each other
    /// </summary>
    protected virtual void Nudge()
    {
        AddForce(new Vector3()
        {
            x = Random.Range(-NudgeForce, NudgeForce),
            y = Random.Range(-NudgeForce, NudgeForce),
        }, ForceMode2D.Impulse);
    }
    [ContextMenu("Show Rigidbody")]
    private void ShowRigidbody()
    {
        rigidbody.hideFlags &= ~(HideFlags.HideInInspector);
    }
    [ContextMenu("Hide Rigidbody")]
    private void HideRigidbody()
    {
        rigidbody.hideFlags |= HideFlags.HideInInspector;
    }
    public enum Scaling
    {
        None = 0,
        Orthographic = 1,
    }
}