using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply adds the current seed to the details field
/// </summary>
public class UpdateRichPresenceWithSeed : UpdateRichPresence
{
    [SerializeField]
    private StringVariable seed = default;

    public override void OnRaised()
    {
        DiscordRichPresence.UpdateInformation(description, updateTime, $"Seed: {seed.Value}");
    }
}
