using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityRenderer : MonoBehaviour
{
    protected Vector2 TargetPosition
    {
        get => targetPosition;
        set => targetPosition = value;
    }
    private Vector2 targetPosition;

    public virtual void UpdatePosition(Vector3 position) => TargetPosition = position;
    public virtual void UpdateColor(Color color) { }
    public virtual void OnStarted() { }
    public virtual void OnEnded() { }
    public virtual void OnTick() { }
}
