using UnityEngine;

public class GameObjectAdder : MonoBehaviour
{
    [SerializeField]
    private GameObjectCollection targetCollection = null;
    
    protected virtual void Awake()
    {
        targetCollection.Add(gameObject);
    }
    protected virtual void OnDestroy()
    {
        targetCollection.Remove(gameObject);
    }
}