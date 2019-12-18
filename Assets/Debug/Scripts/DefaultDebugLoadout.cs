using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables elements by default
/// </summary>
[CreateAssetMenu(fileName = "DefaultDebugElementLoadout.asset", menuName = Utility.MenuItemDebug + "Default Loadout", order = 1000)]
public class DefaultDebugLoadout : ScriptableObject
{
    [SerializeField, Reorderable]
    private DebugElementArray elementsToEnable = new DebugElementArray();

    public void Load()
    {
        foreach (DebugElementBase debugElement in elementsToEnable)
        {
            debugElement.Enable();
        }
    }
    
    [System.Serializable]
    private class DebugElementArray : ReorderableArray<DebugElementBase>
    {
    }
}
