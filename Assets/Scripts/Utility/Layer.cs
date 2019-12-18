using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all available Unity layers
/// </summary>
[System.Flags]
public enum Layer : int
{
    Defualt             = 1 << 0,
    TransparentFX       = 1 << 1,
    IgnoreRaycast       = 1 << 2,
    Water               = 1 << 3,
    UI                  = 1 << 4,
    Player              = 1 << 8,
    Enemy               = 1 << 9,
    Clutter             = 1 << 10,
    
    HostileProjectile   = 1 << 12,
    Environment         = 1 << 13,
    EnvironmentNoBlock  = 1 << 14,
    
    AllEnvironment      = Environment | EnvironmentNoBlock | Clutter,
}
