using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Identifiers for brush bases
/// </summary>
public enum BrushIdentifiers : ushort
{
    // This should *NEVER* be changed. It's serialized, so if you change it, the serialized brushes will be incorrect
    DefaultFloor = 0,
    DefaultWall = 1,
}
