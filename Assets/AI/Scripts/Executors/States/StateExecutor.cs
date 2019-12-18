using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages several states in the hierarchy. Each state should have a <see cref="StateElement"/> which uses a <see cref="StateObject"/> instance as an identifier
/// </summary>
public class StateExecutor : MonoBehaviour
{
    [SerializeField, Reorderable]
    private StateElementList stateElements = new StateElementList();
    
    public void EnableState(StateObject newActiveState)
    {
        foreach (StateElement element in stateElements)
            element.Toggle(newActiveState);
    }
    
    [System.Serializable]
    private class StateElementList : ReorderableArray<StateElement> { }
}
