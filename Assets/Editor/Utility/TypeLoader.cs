using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

public static class TypeLoader
{
    private static Dictionary<Type, List<Type>> LoadedTypes = new Dictionary<Type, List<Type>>();

    public static List<Type> GetTypes(Type baseType, bool getAbstract = false)
    {
        if (!LoadedTypes.ContainsKey(baseType))
        {
            List<Type> allTypes = GetAllTypesFromBaseType(baseType, getAbstract);

            LoadedTypes.Add(baseType, allTypes);
        }

        return LoadedTypes[baseType];
    }
    private static List<Type> GetAllTypesFromBaseType(Type baseType, bool getAbstract)
    {
        return baseType.Assembly.GetTypes()
            .Where(x => baseType.IsAssignableFrom(x) && PollAbstract(x, getAbstract))
            .ToList();
    }
    private static bool PollAbstract(Type type, bool getAbstract)
    {
        if (getAbstract)
            return true;

        return !type.IsAbstract;
    }
}
