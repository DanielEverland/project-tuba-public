using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Loads ability assets from Resources into memory
/// </summary>
public static class AbilityLoader
{
    static AbilityLoader()
    {
        behaviour = Resources.LoadAll<AbilityBehaviour>("").ToList();
        action = Resources.LoadAll<AbilityAction>("").ToList();

        LogData();
    }

    public static IEnumerable<AbilityBehaviour> Behaviour => behaviour;
    public static IEnumerable<AbilityAction> Action => action;

    private static List<AbilityBehaviour> behaviour;
    private static List<AbilityAction> action;

    private static void LogData()
    {
        PrintInfo();
        CheckWarnings();
    }
    private static void PrintInfo()
    {
        string infoString = $"---Loading Parts---\n{behaviour.Count} behaviours, {action.Count} actions";

        Debug.Log(infoString);
    }
    private static void CheckWarnings()
    {
        WarnIfListIsEmpty(behaviour, "Behaviour Parts");
        WarnIfListIsEmpty(action, "Behaviour Actions");
    }
    private static void WarnIfListIsEmpty(ICollection collection, string collectionName)
    {
        if (collection.Count < 1)
        {
            Debug.LogError("No " + collectionName + " are loaded!");
        }
    }
}