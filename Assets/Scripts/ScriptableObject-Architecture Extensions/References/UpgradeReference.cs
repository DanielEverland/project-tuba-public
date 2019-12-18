using UnityEngine;

[System.Serializable]
public sealed class UpgradeReference : BaseReference<UpgradeObject, UpgradeVariable>
{
    public UpgradeReference() : base() { }
    public UpgradeReference(UpgradeObject value) : base(value) { }
}