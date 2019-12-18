using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

/// <summary>
/// Static container for all brush types
/// </summary>
public static class BrushTypes
{
    static BrushTypes()
    {
        AllBrushes = new Dictionary<ushort, BrushBase>();

        IEnumerable<Type> types = typeof(BrushBase).Assembly.GetTypes()
            .Where(x => typeof(BrushBase).IsAssignableFrom(x))
            .Where(x => x.IsAbstract == false);

        foreach (Type type in types)
        {
            BrushBase brush = (BrushBase)System.Activator.CreateInstance(type);

            if (AllBrushes.ContainsKey((ushort)brush.UniqueIdentifier))
            {
                throw new System.InvalidOperationException($"Unique ID for brush already exists!" +
                    $"\nExisting brush is {AllBrushes[(ushort)brush.UniqueIdentifier]} and new brush is {brush}");
            }

            AllBrushes.Add((ushort)brush.UniqueIdentifier, brush);
        }
    }

    public static Dictionary<ushort, BrushBase> AllBrushes { get; private set; }
    public static List<BrushBase> OrderedBrushes => new List<BrushBase>(AllBrushes.Values.OrderBy(x => x.Name));
}
