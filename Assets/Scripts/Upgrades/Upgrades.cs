using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Type = System.Type;

public static class Upgrades
{
    private static UnityEvent onUpgradeChanged = new UnityEvent();
    private static UpgradeObject upgradeObject;
    
    public static List<T> GetAllModifiers<T>() where T : UpgradeModifier
    {
        return new List<T>(upgradeObject.Modifiers
            .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
            .Select(x => (T)x));
    }
    public static void AddListener(UnityAction action)
    {
        onUpgradeChanged.AddListener(action);
    }
    public static void RemoveListener(UnityAction action)
    {
        onUpgradeChanged.RemoveListener(action);
    }
    public static void EquipUpgrade(UpgradeObject upgrade)
    {
        upgradeObject = upgrade;

        onUpgradeChanged.Invoke();
    }
    public static void UnequipUpgrade()
    {
        upgradeObject = null;

        onUpgradeChanged.Invoke();
    }

    public static T GetBestFloatValue<T>(IReadOnlyList<T> collection, System.Func<T, float> func, Comparer comparer) where T : UpgradeModifier
    {
        return GetBestGenericValue(collection, func, CompareFloat, comparer);
    }
    private static TValue GetBestGenericValue<TValue, TComparerType>(IReadOnlyList<TValue> collection, System.Func<TValue, TComparerType> getValue, System.Func<TComparerType, TComparerType, Comparer, bool> compareFunction, Comparer comparer) where TValue : UpgradeModifier
    {
        TValue best = default;

        foreach (TValue item in collection)
        {
            if (best == null)
            {
                best = item;
                continue;
            }

            if (compareFunction(getValue(best), getValue(item), comparer))
                best = item;
        }

        return best;
    }

    private static bool CompareFloat(float a, float b, Comparer comparer)
    {
        switch (comparer)
        {
            case Comparer.Larger:
                return a > b;
            case Comparer.Smaller:
                return a < b;
            default:
                throw new System.NotImplementedException();
        }
    }

    public enum Comparer
    {
        None = 0,
        
        Larger = 1,
        Smaller = 2,
    }

    #region ToRemove
    public const float AllSplashDamageDefaultRadius = 1;
    public const float AllSplashDamageRadiusMultiplier = 1.5f;
    public const float AllVampyricDamageMultiplier = 0.75f;
    public const float AllVampyricDamageHealUpgradeMultiplier = 2;
    #endregion
}
