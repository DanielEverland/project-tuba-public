using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;

/// <summary>
/// AI Entity that will disable certain behaviours when they're not necessary
/// </summary>
public class AIEntity : Entity
{
    [SerializeField]
    private HierarchyCategory hierarchyCategory = HierarchyCategory.Enemies;
    [SerializeField]
    private BehaviourTreeOwner tree;

    private bool IsTreeRunning => tree.graph.isRunning;
    
    protected virtual void Awake()
    {
        HierarchyManager.Add(gameObject, hierarchyCategory);
    }
    protected override void Update()
    {
        base.Update();

        PollBrain();
    }
    private void PollBrain()
    {
        ToggleBrain(ShouldBrainBeActive());
    }
    private bool ShouldBrainBeActive()
    {
        return Health.IsAlive && !PauseState.IsPaused;
    }
    private void ToggleBrain(bool enabled)
    {
        if (IsTreeRunning && !enabled)
        {
            tree.StopBehaviour();
        }
        else if(!IsTreeRunning && enabled)
        {
            tree.StartBehaviour();
        }        
    }
    private void OnEnable()
    {
        AddModifierListener(CheckIfStunned, typeof(StunnedModifier));
    }
    private void OnDisable()
    {
        RemoveModifierListener(CheckIfStunned, typeof(StunnedModifier));
    }
    private void CheckIfStunned()
    {
        if (ContainsModifier(typeof(StunnedModifier)))
        {
            tree.StopBehaviour();
        }
        else
        {
            tree.StartBehaviour();
        }
    }
    private void OnValidate()
    {
        if (tree == null)
            tree = GetComponent<BehaviourTreeOwner>();
    }
}
