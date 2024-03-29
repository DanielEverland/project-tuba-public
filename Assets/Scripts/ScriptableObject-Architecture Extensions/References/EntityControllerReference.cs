using UnityEngine;

[System.Serializable]
public sealed class EntityControllerReference : BaseReference<EntityController, EntityControllerVariable>
{
    public EntityControllerReference() : base() { }
    public EntityControllerReference(EntityController value) : base(value) { }
}