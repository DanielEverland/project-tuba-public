using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for a given phase during a boss encounter
/// </summary>
public class BossFightPhase : MonoBehaviour
{
    [SerializeField]
    private bool destroyWhenCompleted = false;
    [SerializeField]
    private FloatReference duration = new FloatReference(60);
    [SerializeField]
    private UnityEvent OnPhaseStart = new UnityEvent();
    [SerializeField]
    private UnityEvent OnPhaseEnd = new UnityEvent();

    public bool DestroyWhenCompleted => destroyWhenCompleted;
    public float Duration => duration.Value;

    protected bool IsPhaseActive { get; private set; }
    
    public virtual void StartPhase()
    {
        IsPhaseActive = true;

        gameObject.SetActive(true);
    }
    public virtual void EndPhase()
    {
        IsPhaseActive = false;

        gameObject.SetActive(false);
    }
    protected virtual void PhaseUpdate() { }

    private void Update()
    {
        if (IsPhaseActive)
            PhaseUpdate();
    }
    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }
    protected virtual void OnEnable()
    {
        OnPhaseStart.Invoke();
    }
    protected virtual void OnDisable()
    {
        OnPhaseEnd.Invoke();
    }
}
