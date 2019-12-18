using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for querying all active <see cref="Tags"/>
/// </summary>
public abstract class TagQueryer : CallbackMonobehaviour
{
    [SerializeField, Reorderable]
    private TagList include = default;

    [SerializeField, Reorderable]
    private TagList exclude = default;

    protected List<Tags> QueryTags()
    {
        List<Tags> result = new List<Tags>();

        for (int i = 0; i < include.Length; i++)
        {
            if (Tags.TryGetAllOfType(include[i], out var activeTagsFromType))
            {
                result.AddRange(activeTagsFromType
                    .Where(x => !exclude.Overlaps(x)));
            }
        }

        return result;
    }
}
