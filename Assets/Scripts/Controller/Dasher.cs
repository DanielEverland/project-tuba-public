using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dasher : MonoBehaviour
{
    [SerializeField]
    protected Entity entity = default;
    [SerializeField]
    protected EntityController characterController = null;
    [SerializeField]
    protected TrailRenderer[] trailRenderers = null;
    [SerializeField]
    protected FloatReference dashLength = new FloatReference(5);
    [SerializeField]
    protected FloatReference dashTime = new FloatReference(0.3f);
    [SerializeField]
    protected AnimationCurve positionCurve = new AnimationCurve(new Keyframe(0, 0, 0, 3), new Keyframe(1, 1, 0, 0));
    [SerializeField]
    private UnityEvent onDashingStarted = new UnityEvent();
    [SerializeField]
    private UnityEvent onDashingFinished = new UnityEvent();

    public bool IsDashing { get; protected set; }

    protected float StartTime { get; set; }
    protected Vector2 StartPosition { get; set; }
    protected Vector2 TargetPosition { get; set; }
    
    public virtual void Dash(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;
        
        IsDashing = true;

        StartTime = Time.time;
        StartPosition = transform.position;
        TargetPosition = (Vector2)transform.position + direction * dashLength.Value;

        DashingStarted();
    }
    protected virtual void Update()
    {
        if (!IsDashing)
            return;

        float timePassed = Time.time - StartTime;

        if(timePassed > dashTime.Value)
        {
            IsDashing = false;

            DashingFinished();
        }
        else
        {
            float percentage = Mathf.Clamp01(timePassed / dashTime.Value);
            float modifiedPercentage = positionCurve.Evaluate(percentage);

            Vector2 position = Vector2.Lerp(StartPosition, TargetPosition, modifiedPercentage);
            Vector2 delta = position - (Vector2)transform.position;

            characterController.Move(delta / Time.fixedDeltaTime, EntityController.Scaling.None);
        }        
    }
    protected virtual void DashingStarted()
    {
        onDashingStarted.Invoke();

        EnableTrailRenderers();      
    }
    protected virtual void DashingFinished()
    {
        onDashingFinished.Invoke();

        DisableTrailRenderers();        
    }    
    private void EnableTrailRenderers()
    {
        ToggleTrailRenderers(true);
    }
    private void DisableTrailRenderers()
    {
        ToggleTrailRenderers(false);
    }
    private void ToggleTrailRenderers(bool value)
    {
        for (int i = 0; i < trailRenderers.Length; i++)
            trailRenderers[i].emitting = value;
    }
    private void OnValidate()
    {
        if(entity == null)
        {
            if(gameObject.TryGetEntity(out Entity entity))
            {
                this.entity = entity;
            }
        }
    }
}
