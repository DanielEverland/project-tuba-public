using UnityEngine;

[System.Serializable]
public sealed class ThemeReference : BaseReference<Theme, ThemeVariable>
{
    public ThemeReference() : base() { }
    public ThemeReference(Theme value) : base(value) { }
}