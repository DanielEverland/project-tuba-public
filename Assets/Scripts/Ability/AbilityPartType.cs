using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum AbilityPartType
{
    None        = 0b0000,

    Behaviour   = 0b0001,
    Action      = 0b0010,
}
