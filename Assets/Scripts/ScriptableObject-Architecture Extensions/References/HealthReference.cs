using UnityEngine;

[System.Serializable]
public sealed class HealthReference : BaseReference<Health, HealthVariable>
{
    public HealthReference() : base() { }
    public HealthReference(Health value) : base(value) { }
}