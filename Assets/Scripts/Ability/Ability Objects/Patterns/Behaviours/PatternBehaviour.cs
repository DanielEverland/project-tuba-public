using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternBehaviour : ScriptableObject
{
    public PatternObject Pattern { get; set; }

    protected float StartTime { get; set; }

    public abstract void Update();

    public virtual void Initialize()
    {
        StartTime = Time.time;
    }
    protected void Evaluate(Action<PatternElement> action)
    {
        for (int i = 0; i < Pattern.Elements.Count; i++)
        {
            PatternElement element = Pattern.Elements[i];

            // This'll happen if it has been destroyed
            if (element != null)
                action(element);
        }
    }
}