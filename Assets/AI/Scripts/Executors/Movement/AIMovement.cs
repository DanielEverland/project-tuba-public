using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for AI wanting to use an <see cref="EntityController"/>
/// </summary>
public class AIMovement : MonoBehaviour, IMoveTowardsInteractable, IMoveTowardsPosition
{
    [SerializeField]
    private EntityController controller;
    [SerializeField]
    private LayerMask blockingLayers = (int)Layer.Environment | (int)Layer.EnvironmentNoBlock;
    [SerializeField]
    private FloatReference movementSpeed = new FloatReference(5);

    public float MovementSpeed
    {
        get => movementSpeed.Value;
        set => movementSpeed.Value = value;
    }

    private const float StuckMinTime = 2;
    private const float PositionComparerPrecision = 0.1f;

    private bool HasPath => currentPath != null;
    private IPath currentPath;
    private float lastPositionMoved;
    private float lastTimeChecked;
    private Vector2 previousPosition;

    private void FixedUpdate()
    {
        if (!HasPath)
            return;

        if (DirectPathToTarget())
        {
            MoveTo(currentPath.TargetPosition);
        }
        else
        {
            MoveWithPathfinding();
        }
    }    
    public void MoveTowards(Vector2 target)
    {
        if(currentPath?.TargetPosition != target)
            currentPath = PathHandler.GetPath(transform.position, target);
    }
    public void MoveTowards(Interactable interactable)
    {
        currentPath = PathHandler.GetPath(interactable);
    }
    public void ClearTarget()
    {
        currentPath = null;
    }
    public bool IsStuck()
    {
        if(IsDifferent(previousPosition, transform.position))
        {
            lastPositionMoved = Time.time;
        }
        else if(Time.time - lastPositionMoved > StuckMinTime && Time.time - lastTimeChecked < StuckMinTime)
        {
            return true;
        }

        lastTimeChecked = Time.time;
        previousPosition = transform.position;

        return false;
    }

    private void MoveWithPathfinding()
    {
        Vector2 targetPosition = currentPath.GetNextPosition(transform.position);

        MoveTo(targetPosition);
    }
    private void MoveTo(Vector2 targetPosition)
    {
        controller.MoveTo(targetPosition, MovementSpeed);
    }
    private bool DirectPathToTarget()
    {
        // Here we simply test to see if there's a straight path to the target.
        return Physics2D.RaycastNonAlloc(transform.position, GetDirectionToTarget(), Utility.HitBuffer, GetDistanceToTarget(), blockingLayers) == 0;
    }
    private Vector2 GetDirectionToTarget()
    {
        return (currentPath.TargetPosition - (Vector2)transform.position).normalized;
    }
    private float GetDistanceToTarget()
    {
        return (currentPath.TargetPosition - (Vector2)transform.position).magnitude;
    }
    private bool IsDifferent(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b) > PositionComparerPrecision;
    }
    

    private void OnValidate()
    {
        Entity entity = Utility.GetEntityFromHierarchyTraversal(gameObject);
        controller = entity.GetComponent<EntityController>();
    }
}