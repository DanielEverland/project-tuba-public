using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PatternComponent : ScriptableObject
{
    public void SpawnChildren(PatternElement prefab, PatternObject pattern)
    {
        System.Action<Vector2> spawnChildDelegate = x =>
        {
            PatternElement instance = Instantiate(prefab);
            instance.transform.position = Utility.ScaleToOrthographicVector(x);
            instance.AssignStartingPosition(x);
            instance.name = $"Pattern ({pattern.Elements.Count})";
                        
            pattern.AddElement(instance);
        };

        DoSpawnChildren(spawnChildDelegate);
    }
    protected abstract void DoSpawnChildren(System.Action<Vector2> onSpawnChild);
}