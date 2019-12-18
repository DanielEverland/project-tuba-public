using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows AI to see things within the environment
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Entity entity = default;
    [SerializeField]
    private bool inheritEntityTags = true;
    [SerializeField, Reorderable]
    private TagList tags = default;
    
    public Entity Entity => entity;

    private TagList Tags
    {
        get
        {
            if (inheritEntityTags && entity != null)
                return Entity.Tags;
            else
                return tags;
        }
    }
    
    public bool ContainsTag(TagType tag)
    {
        return Tags.Contains(tag);
    }
    public bool ContainsAny(IList<TagType> tagList)
    {
        for (int i = 0; i < tagList.Count; i++)
        {
            if (Tags.Contains(tagList[i]))
                return true;
        }

        return false;
    }
    public bool ContainsAll(IList<TagType> tagList)
    {
        for (int i = 0; i < tagList.Count; i++)
        {
            if (!Tags.Contains(tagList[i]))
                return false;
        }

        return true;
    }
    private void OnValidate()
    {
        entity = Utility.GetEntityFromHierarchyTraversal(gameObject);
    }
}
