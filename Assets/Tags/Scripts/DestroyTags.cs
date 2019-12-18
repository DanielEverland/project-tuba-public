using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys all <see cref="TagType"/> when called
/// </summary>
public class DestroyTags : TagQueryer
{
    public override void OnRaised()
    {
        List<Tags> tags = QueryTags();

        for (int i = 0; i < tags.Count; i++)
            tags[i].DestroyObject();
    }
}
