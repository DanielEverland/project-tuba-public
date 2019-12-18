using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

public static class PatternLoader
{
    public static List<Type> Components
    {
        get
        {
            if (components == null)
                components = new List<Type>(LoadOfType<PatternComponent>());

            return components;
        }
    }
    private static List<Type> components;

    public static List<Type> Behaviours
    {
        get
        {
            if (behaviours == null)
                behaviours = new List<Type>(LoadOfType<PatternBehaviour>());

            return behaviours;
        }
    }
    private static List<Type> behaviours;

    private static IEnumerable<Type> LoadOfType<T>()
    {
        return typeof(T).Assembly.GetTypes()
            .Where(x => typeof(T).IsAssignableFrom(x)
                && !x.IsAbstract
                && x != typeof(T));
    }
}