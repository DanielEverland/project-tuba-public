using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper for brush data in a <see cref="BrushBase"/>
/// </summary>
[System.Serializable]
public class BrushData
{
    public BrushBase Brush => BrushTypes.AllBrushes[uniqueIdentifier];

    [SerializeField, HideInInspector]
    private ushort uniqueIdentifier = default;
}
