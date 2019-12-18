using System.Collections;
using UnityEngine;

/// <summary>
/// Will enabled and disabled phases sequentially throughout a boss encounter
/// </summary>
public class BossFightPhaseHandler : MonoBehaviour
{
    [SerializeField]
    [Reorderable]
    private PhaseList phaseEntries = default;

    private float currentDuration = 0;
    private int currentPhaseIndex;

    private void Awake()
    {
        if(phaseEntries.Count == 0)
        {
            Debug.LogWarning($"No phases have been assigned to {name}", this);
            enabled = false;
        }
    }
    private IEnumerator Start()
    {
        yield return 0;

        EnablePhase(phaseEntries[currentPhaseIndex]);
    }
    private void Update()
    {
        if (PauseState.IsPaused)
            return;

        UpdateDuration();

        if(currentDuration >= phaseEntries[currentPhaseIndex].Duration)
        {
            MoveToNextPhase();
        }
    }
    private void UpdateDuration()
    {
        currentDuration += Time.deltaTime;
    }
    private void MoveToNextPhase()
    {
        ResetDuration();


        BossFightPhase currentPhase = phaseEntries[currentPhaseIndex];
        DisablePhase(currentPhase);

        if (currentPhase.DestroyWhenCompleted)
        {
            phaseEntries.RemoveAt(currentPhaseIndex);
            Destroy(currentPhase.gameObject);
        }
        else
        {
            NextIndex();
        }
        
        EnablePhase(phaseEntries[currentPhaseIndex]);
    }
    private void DisablePhase(BossFightPhase phase)
    {
        phase.EndPhase();
    }
    private void EnablePhase(BossFightPhase phase)
    {
        phase.StartPhase();
    }
    private void ResetDuration()
    {
        currentDuration = 0;
    }
    private void NextIndex()
    {
        if(currentPhaseIndex >= phaseEntries.Count - 1)
        {
            currentPhaseIndex = 0;
        }
        else
        {
            currentPhaseIndex++;
        }
    }

    [System.Serializable]
    private class PhaseList : ReorderableArray<BossFightPhase> { }
}
