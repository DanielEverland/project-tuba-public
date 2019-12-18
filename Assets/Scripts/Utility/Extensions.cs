using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static UnityEngine.Random;

public static class Extensions
{
    /// <summary>
    /// Overwrites pair at <typeparamref name="TKey"/> if one exists. If not, add new pair
    /// </summary>
    public static void Overwrite<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if(dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
    /// <summary>
    /// Returns direction given angle in degrees
    /// </summary>
    public static Vector2 GetDirection(this float angle)
    {
        return new Vector2()
        {
            x = Mathf.Cos(angle * Mathf.Deg2Rad),
            y = Mathf.Sin(angle * Mathf.Deg2Rad),
        };
    }
    /// <summary>
    /// Returns the angle of the normalized vector
    /// </summary>
    public static float GetAngle(this Vector2 vector)
    {
        vector.Normalize();
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// Returns the angle of the normalized vector
    /// </summary>
    public static float GetAngle(this Vector3 vector)
    {
        vector.Normalize();
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// Tries to get the entity associated with this <paramref name="transform"/>
    /// </summary>
    /// <param name="transform"></param>
    public static bool TryGetEntity(this Transform transform, out Entity entity)
    {
        return transform.gameObject.TryGetEntity(out entity);
    }
    /// <summary>
    /// Tries to get the entity associated with this <paramref name="gameObject"/> using <see cref="Interactable"/>
    /// </summary>
    /// <param name="gameObject"></param>
    public static bool TryGetEntity(this GameObject gameObject, out Entity entity)
    {
        entity = null;
        Interactable interactable = gameObject.GetComponent<Interactable>();

        if(interactable != null)
        {
            if(interactable.Entity != null)
            {
                entity = interactable.Entity;
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Gets the entity associated with this <paramref name="transform"/>
    /// </summary>
    /// <param name="transform"></param>
    public static Entity GetEntity(this Transform transform)
    {
        return transform.gameObject.GetEntity();
    }
    /// <summary>
    /// Gets the entity associated with this <paramref name="gameObject"/>
    /// </summary>
    /// <param name="gameObject"></param>
    public static Entity GetEntity(this GameObject gameObject)
    {
        Interactable interactable = gameObject.GetComponent<Interactable>();

        if (interactable != null)
            return interactable.Entity;

        System.EntityException exception = new System.EntityException(gameObject);
        Debug.LogException(exception, gameObject);

        return null;
    }
    /// <summary>
    /// Creates a deep copy of <paramref name="obj"/>
    /// </summary>
    public static T DeepCopy<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    /// <summary>
    /// Instantiates all entries in a list
    /// </summary>
    public static void Instantiate<T>(this List<T> list) where T : Object
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Object.Instantiate(list[i]);
        }
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector4 RoundToNearest(this Vector4 vector, float nearest)
    {
        return new Vector4()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
            z = vector.z.RoundToNearest(nearest),
            w = vector.w.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector3 RoundToNearest(this Vector3 vector, float nearest)
    {
        return new Vector3()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
            z = vector.z.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector2 RoundToNearest(this Vector2 vector, float nearest)
    {
        return new Vector2()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static float RoundToNearest(this float value, float nearest)
    {
        return Mathf.Round(value / nearest) * nearest;
    }
    /// <summary>
    /// Returns the next value from the enum
    /// </summary>
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(System.String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
    /// <summary>
    /// Like clamp, but will wrap around instead of truncating
    /// </summary>
    public static float Wrap(this float value, float min, float max)
    {
        if (value > max)
            return min + (value - max) - 1;
        else if (value < min)
            return max - (min - value) + 1;

        return value;
    }
    /// <summary>
    /// Like clamp, but will wrap around instead of truncating
    /// </summary>
    public static int Wrap(this int value, int min, int max)
    {
        if (value > max)
            return min + (value - max) - 1;
        else if (value < min)
            return max - (min - value) + 1;

        return value;
    }
    /// <summary>
    /// Returns a random item from the enumerable
    /// </summary>
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable).Random();
    }
    /// <summary>
    /// Returns a random item from the list
    /// </summary>
    public static T Random<T>(this IList<T> list)
    {
        return list[Range(0, list.Count)];
    }
}