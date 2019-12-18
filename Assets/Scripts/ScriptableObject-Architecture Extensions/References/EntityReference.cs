using UnityEngine;

[System.Serializable]
public sealed class EntityReference : BaseReference<Entity, EntityVariable>
{
    public EntityReference() : base() { }
    public EntityReference(Entity value) : base(value) { }
}