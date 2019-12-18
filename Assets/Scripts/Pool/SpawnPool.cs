using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides an interface for spawning <see cref="Pool"/> objects
/// </summary>
public class SpawnPool : MonoBehaviour, IEventHandler<float>
{
    [SerializeField]
    private Pool prefab = default;
    [SerializeField]
    private bool once = true;

    private bool hasSpawned = false;

    public void Raise(float value) => Spawn(value);
    public void Spawn(float radius)
    {
        SpawnAtPosition(radius, transform.position);
    }
    public void SpawnAtPosition(float radius, Vector2 position)
    {
        if (!enabled || (hasSpawned && once))
            return;

        hasSpawned = true;

        Pool instance = InstantiatePoolObject(radius);
        instance.transform.position = position;
    }
    private Pool InstantiatePoolObject(float radius)
    {
        Pool instance = HierarchyManager.Instantiate(prefab, HierarchyCategory.Effects);
        instance.Radius = radius;

        return instance;
    }
}
