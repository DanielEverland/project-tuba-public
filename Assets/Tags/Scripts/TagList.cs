using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TagList : ReorderableArray<TagType>
{
    /// <summary>
    /// Returns whether any tags in this taglist are present in <paramref name="other"/>
    /// </summary>
    public bool Overlaps(TagList other)
    {
        for (int i = 0; i < Count; i++)
        {
            if (other.Contains(this[i]))
                return true;
        }

        return false;
    }
}