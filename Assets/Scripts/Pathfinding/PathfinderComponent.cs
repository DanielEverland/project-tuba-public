using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will manage all pathfinding related operations for an entity
/// </summary>
public class PathfinderComponent : MonoBehaviour
{
    //[SerializeField]
    //private Entity entity = default;

    //public Vector2 CurrentTargetPosition { get; private set; }
    //public bool HasPath => cachedPath?.Count > 0;

    //protected Vector2 PositionOfTarget => targetEntity.Value.transform.position;
    //protected Entity Entity => entity;

    //private Vector2 previousCalculatePosition = default;
    //private LinkedList<Vector2> cachedPath = default;
    
    //private void Awake()
    //{
    //    this.enabled = false;
    //}
    //private void OnEnable()
    //{
    //    Recalculate();
    //}
    //public void Move(EntityController controller, float speed)
    //{
    //    // Move in the direction of our current target.
    //    Vector2 delta = CurrentTargetPosition - (Vector2)transform.position;
    //    controller.Move(delta.normalized * speed);
    //}
    //private void Update()
    //{
    //    if((previousCalculatePosition - PositionOfTarget).sqrMagnitude >= MinTargetDeltaRecalculate * MinTargetDeltaRecalculate)
    //    {
    //        Recalculate();
    //    }

    //    if ((CurrentTargetPosition - (Vector2)transform.position).sqrMagnitude <= 0.1f)
    //        CurrentTargetPosition = GetNextTargetPosition();

    //    if (HasPath)
    //        Debug.DrawLine(transform.position, CurrentTargetPosition, Color.cyan);
    //}
    //private void Recalculate()
    //{
    //    cachedPath = PathFinding.GetPath(transform.position, PositionOfTarget);
    //    previousCalculatePosition = PositionOfTarget;
    //    CurrentTargetPosition = GetNextTargetPosition();
    //}
    //private Vector2 GetNextTargetPosition()
    //{
    //    if (cachedPath.Count == 0)
    //        return PositionOfTarget;

    //    Vector2 position = Utility.HexagonalToWorldPosition(cachedPath.First.Value);

    //    position += new Vector2(
    //        Random.Range(-PositionOffsetRange, PositionOffsetRange),
    //        Random.Range(-PositionOffsetRange, PositionOffsetRange));

    //    cachedPath.RemoveFirst();

    //    return position;
    //}
    //private void OnValidate()
    //{
    //    entity = Utility.GetEntityFromHierarchyTraversal(gameObject);
    //}
}
