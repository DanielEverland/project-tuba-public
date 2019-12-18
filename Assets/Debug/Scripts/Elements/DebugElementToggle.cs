using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A debug element that can be toggled on or off
/// </summary>
public abstract class DebugElementToggle : DebugElementBase
{
    public bool IsOn { get; private set; }
        
    public void Toggle(bool value)
    {
        if(value)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
    public override void Enable()
    {
        if (IsOn)
            Disable();

        IsOn = true;
        GlobalCallbacks.AddUpdateListener(OnDebugElementUpdate);
        GlobalCallbacks.AddRenderObjectListener(OnDebugElementRenderObject);

        OnDebugElementEnable();
    }
    public void Disable()
    {
        IsOn = false;
        GlobalCallbacks.RemoveUpdateListener(OnDebugElementUpdate);
        GlobalCallbacks.RemoveRenderObjectListener(OnDebugElementRenderObject);

        OnDebugElementDisable();
    }

    public virtual void OnDebugElementEnable() { }
    public virtual void OnDebugElementDisable() { }
    public virtual void OnDebugElementUpdate() { }
}
