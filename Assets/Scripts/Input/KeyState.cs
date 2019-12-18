using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Specifies what the state of a <see cref="KeyCode"/> is in 
/// </summary>
public enum KeyState
{
    None    = 0b0000,

    Down    = 0b0001,
    Stay    = 0b0010,
    Up      = 0b0100,
}
