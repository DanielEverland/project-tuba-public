using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAdder : MonoBehaviour
{
    [SerializeField]
    private EntityCollection targetCollection = null;
    [SerializeField]
    protected Entity thisEntity = null;
    
    protected virtual void Awake()
    {
        targetCollection.Add(thisEntity);
    }
    protected virtual void OnDestroy()
    {
        targetCollection.Remove(thisEntity);
    }
    protected void OnValidate()
    {
        if(thisEntity == null)
            thisEntity = gameObject.GetEntity();
    }
}
