using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum AbilityStateDelta
{
    None                = 0b0000,
    Reloading           = 0b0001,
    Fired               = 0b0010,
    HasPersistentObject = 0b0100,
}
