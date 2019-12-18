using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour, IReadOnlyList<TagType>
{
    [SerializeField, Reorderable]
    private TagList tags = new TagList();

    public int Count => tags.Count;
    public TagType this[int index] => tags[index];
    
    public void AddType(TagType type)
    {
        tags.Add(type);

        ActiveTagContainer.AddTags(this);
    }
    public bool ContainsType(TagType type)
    {
        return tags.Contains(type);
    }
    public void DestroyObject()
    {
        ITagDestroyedHandler[] destroyHandler = GetComponents<ITagDestroyedHandler>();

        if(destroyHandler.Length != 0)
        {
            for (int i = 0; i < destroyHandler.Length; i++)
                destroyHandler[i].Destroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        ActiveTagContainer.AddTags(this);
    }
    private void OnDisable()
    {
        ActiveTagContainer.RemoveTags(this);
    }

    public IEnumerator<TagType> GetEnumerator()
    {
        return tags.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return tags.GetEnumerator();
    }

    public static bool TryGetAllOfType(TagType type, out IReadOnlyCollection<Tags> tags) => ActiveTagContainer.TryGetAllOfType(type, out tags);
    public static IReadOnlyCollection<Tags> GetAllOfType(TagType type) => ActiveTagContainer.GetAllOfType(type);
    public static bool ContainsType(Entity obj, IEnumerable<TagType> enumerable) => ContainsType(obj.gameObject, enumerable);
    public static bool ContainsType(Entity obj, TagType type) => ContainsType(obj.gameObject, type);
    public static bool ContainsType(GameObject obj, TagType type)
    {
        Tags tags = obj.GetComponent<Tags>();

        if (tags == null)
            return false;

        return obj.GetComponentInParent<Tags>().ContainsType(type);
    }
    public static bool ContainsType(GameObject obj, IEnumerable<TagType> enumerable)
    {
        Tags tags = obj.GetComponent<Tags>();

        if (tags == null)
            return false;

        foreach (TagType tag in enumerable)
        {
            if (tags.ContainsType(tag))
                return true;
        }

        return false;
    }

    public static implicit operator TagList(Tags tags)
    {
        return tags.tags;
    }
}
