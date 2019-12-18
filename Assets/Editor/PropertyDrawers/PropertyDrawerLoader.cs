using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

public static class PropertyDrawerLoader
{
    public static Dictionary<Type, PropertyDrawer> Drawers
    {
        get
        {
            if (drawers == null)
                LoadDrawers();

            return drawers;
        }
    }
    private static Dictionary<Type, PropertyDrawer> drawers;

    private static readonly BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic;
    
    private static void LoadDrawers()
    {
        drawers = new Dictionary<Type, PropertyDrawer>();

        IEnumerable<PropertyDrawer> allDrawers = typeof(PatternLoader).Assembly.GetTypes()
            .Where(x => typeof(PropertyDrawer).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => Activator.CreateInstance(x) as PropertyDrawer);

        foreach (PropertyDrawer drawer in allDrawers)
        {
            object[] attributes = drawer.GetType().GetCustomAttributes(typeof(CustomPropertyDrawer), false);

            if (attributes.Length != 0)
            {
                CustomPropertyDrawer drawerAttribute = attributes[0] as CustomPropertyDrawer;
                Type targetType = (Type)typeof(CustomPropertyDrawer).GetField("m_Type", FieldFlags).GetValue(drawerAttribute);

                if (targetType.IsAbstract == false)
                {
                    drawers.Add(targetType, drawer);
                }
            }
        }
    }
}