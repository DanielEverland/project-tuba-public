using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will update Discord Rich Presence Data
/// </summary>
public class UpdateRichPresence : CallbackMonobehaviour
{
    [SerializeField]
    protected bool updateTime = default;
    [SerializeField]
    protected string description = default;
    
    public override void OnRaised()
    {
        DiscordRichPresence.UpdateInformation(description, updateTime);
    }
}
