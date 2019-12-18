using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class AttackerSpawnPattern : AttackerBase
{
    [SerializeField]
    private Entity owner = default;
    [SerializeField]
    private Pattern pattern = default;
    
    private PatternObject currentObj = default;

    protected override void DoStart()
    {
        currentObj = pattern.Spawn();
        currentObj.transform.position = TargetPosition;
    }
    protected override Status UpdateState()
    {
        return currentObj == null ? Status.Success : Status.Running;
    }
    private void OnValidate()
    {
        if (owner == null)
            owner = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}
