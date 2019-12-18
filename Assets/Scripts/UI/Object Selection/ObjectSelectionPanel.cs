using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionPanel : MonoBehaviour, IObjectSelectionPanel
{
    [SerializeField]
    private ObjectSelectionElement elementPrefab = default;
    [SerializeField]
    private Transform layoutParent = default;

    protected virtual System.Action<IObjectDescription> Callback { get; private set; }

    public void Initialize(IEnumerable<IObjectDescription> allEntries, System.Action<IObjectDescription> callback, bool includeNull = false)
    {
        Callback = callback;

        SpawnElements(allEntries, includeNull);
    }
    public void SelectObject(IObjectDescription obj)
    {
        Callback(obj);

        Destroy(gameObject);
    }
    protected virtual void SpawnElements(IEnumerable<IObjectDescription> allEntries, bool includeNull)
    {
        if(includeNull)
        {
            SpawnElement(null);
        }

        foreach (IObjectDescription objectDescription in allEntries)
        {
            SpawnElement(objectDescription);
        }
    }
    protected virtual void SpawnElement(IObjectDescription objectDescription)
    {
        ObjectSelectionElement element = Instantiate(elementPrefab);
        element.transform.SetParent(layoutParent);

        element.Initialize(this, objectDescription);
    }
}
