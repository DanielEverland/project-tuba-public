using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Discord.DiscordRpc;

/// <summary>
/// Handles everything related to Discord Rich Presence implementations
/// </summary>
public class DiscordRichPresence : MonoBehaviour
{
    private static bool HasBeenInitialized = false;
    private const string ClientID = "538447433357590559";
    private const string LargeImageKey = "icon";

    // TODO: Implement this when the game is on Steam
    private const string SteamID = default;
    
    public static void UpdateInformation(string description, bool resetTime = true, string state = null)
    {
        if (!HasBeenInitialized)
        {
            if (!Application.isEditor)
            {
                Debug.LogWarning("Trying to update Discord Rich Presence, but it hasn't been initialized!");
            }

            return;
        }

        RichPresence presence = new RichPresence()
        {
            details = description,
            largeImageKey = LargeImageKey,
            largeImageText = "Project TUBA Icon",
        };

        if (resetTime)
            presence.startTimestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();

        if (state != null)
            presence.state = state;


        UpdatePresence(presence);
    }
    
    private void Awake()
    {
        if (HasBeenInitialized)
        {
            Destroy(this);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            HasBeenInitialized = true;

            Connect();
        }
    } 
    private void Connect()
    {
        EventHandlers eventHandlers = new EventHandlers();

        eventHandlers.readyCallback = OnReadyInfo;
        eventHandlers.disconnectedCallback += OnDisconnectedInfo;
        eventHandlers.errorCallback += OnErrorInfo;
        eventHandlers.joinCallback += OnJoinInfo;
        eventHandlers.spectateCallback += OnSpectateInfo;
        eventHandlers.requestCallback += OnRequestInfo;

        Initialize(ClientID, ref eventHandlers, true, SteamID);
    }
    private void OnReadyInfo(ref DiscordUser user)
    {
        Log($"User {user} Is Ready");
    }
    private void OnDisconnectedInfo(int errorCode, string message)
    {
        Log($"User Disconnected {errorCode}: {message}");
    }
    private void OnErrorInfo(int errorCode, string message)
    {
        Log($"Error Received {errorCode}: {message}");
    }
    private void OnJoinInfo(string secret)
    {
        Log($"Received Join Info: {secret}");
    }
    private void OnSpectateInfo(string secret)
    {
        Log($"Received Spectate Info: {secret}");
    }
    private void OnRequestInfo(ref DiscordUser request)
    {
        Log($"Received Request Info From: {request}");
    }
    private void Log(object message)
    {
        Debug.Log($"Discord Rich Presence: {message.ToString()}");
    }
    private void OnApplicationQuit()
    {
        Shutdown();
    }
}
