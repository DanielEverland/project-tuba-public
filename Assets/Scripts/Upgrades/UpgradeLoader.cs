using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads assets from disk into memory
/// </summary>
public static class UpgradeLoader
{
    static UpgradeLoader()
    {
        upgrades = Resources.LoadAll<UpgradeObject>("").ToList();

        LogData();
    }

    public static IEnumerable<UpgradeObject> Upgrades => upgrades;

    private static List<UpgradeObject> upgrades;

    private static void LogData()
    {
        PrintInfo();
        CheckWarnings();
    }
    private static void PrintInfo()
    {
        string infoString = $"---Loading Upgrades---\n{upgrades.Count} upgrades";

        Debug.Log(infoString);
    }
    private static void CheckWarnings()
    {
        WarnIfListIsEmpty(upgrades, "Upgrades");
    }
    private static void WarnIfListIsEmpty(ICollection collection, string collectionName)
    {
        if (collection.Count < 1)
        {
            Debug.LogError("No " + collectionName + " are loaded!");
        }
    }
}
