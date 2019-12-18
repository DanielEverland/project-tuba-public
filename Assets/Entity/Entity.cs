using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Tags))]
public partial class Entity : MonoBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private EntityController entityController;
    [SerializeField]
    private Tags tags;

    public Health Health => health;
    public EntityController EntityController => entityController;
    public Tags Tags => tags;

    private List<GameObject> ownedObjects = new List<GameObject>();
    
    private void Awake()
    {
        if (health == null)
            throw new System.NullReferenceException($"No health component attached to {name}");
    }
    public void AddIFrame(float duration)
    {
        AddModifier(typeof(InvincibleModifier), duration);
    }
    /// <summary>
    /// An owned object will be destroyed when the owner is destroyed
    /// </summary>
    public void AddOwnedObject(GameObject obj)
    {
        ownedObjects.Add(obj);
    }
    public void RemoveOwnedObject(GameObject obj)
    {
        ownedObjects.Remove(obj);
    }
    private void OnDestroy()
    {
        for (int i = ownedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(ownedObjects[i]);
        }
    }
    private void OnValidate()
    {
        if (health == null)
            health = GetComponent<Health>();

        if (entityController == null)
            entityController = GetComponent<EntityController>();

        if (tags == null)
            tags = GetComponent<Tags>();
    }
}
