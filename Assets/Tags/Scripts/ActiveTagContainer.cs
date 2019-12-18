using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When a <see cref="GameObject"/> with a <see cref="Tags"/> component it enabled, it adds itself to this list
/// </summary>
public static class ActiveTagContainer
{
    private static Dictionary<TagType, HashSet<Tags>> activeTags = new Dictionary<TagType, HashSet<Tags>>();

    public static void AddTags(Tags container)
    {
        for (int i = 0; i < container.Count; i++)
            AddTag(container[i], container);
    }
    public static void RemoveTags(Tags container)
    {
        for (int i = 0; i < container.Count; i++)
            RemoveTag(container[i], container);
    }
    public static bool TryGetAllOfType(TagType type, out IReadOnlyCollection<Tags> tags)
    {
        tags = null;

        if (activeTags.ContainsKey(type))
        {
            tags = GetAllOfType(type);
            return true;
        }
        else
        {
            return false;
        }
    }
    public static IReadOnlyCollection<Tags> GetAllOfType(TagType type)
    {
        if (activeTags.ContainsKey(type))
        {
            return activeTags[type];
        }
        else
        {
            throw new System.NullReferenceException($"No tags of type {type}");
        }
    }

    private static void AddTag(TagType type, Tags container)
    {
        if (!activeTags.ContainsKey(type))
            AddNewTagTypeEntry(type);

        if (activeTags[type].Contains(container))
        {
            throw new System.ArgumentException($"Tag container {container} has already been added to tag type {type}");
        }
        else
        {
            activeTags[type].Add(container);
        }        
    }
    private static void RemoveTag(TagType type, Tags container)
    {
        if (!activeTags.ContainsKey(type))
            throw new System.ArgumentException($"Attempted to add container {container} to type {type}, however, " +
                $"the type doesn't exist in the dictionary");

        if (activeTags[type].Contains(container))
        {
            activeTags[type].Remove(container);
        }
        else
        {
            throw new System.ArgumentException($"Attempted to remove {container} from type {type}, " +
                $"however, the container doesn't exist in the dictionary");
        }

        if (activeTags[type].Count == 0)
            RemoveTagTypeEntry(type);
    }
    private static void AddNewTagTypeEntry(TagType type)
    {
        activeTags.Add(type, new HashSet<Tags>());
    }
    private static void RemoveTagTypeEntry(TagType type)
    {
        activeTags.Remove(type);
    }
}
