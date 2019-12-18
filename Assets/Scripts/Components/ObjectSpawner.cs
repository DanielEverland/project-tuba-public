using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private bool alignPosition = true;
    [SerializeField]
    private bool alignRotation = true;
    [SerializeField]
    private GameObject prefab = null;
    
    // UnityEvents cannot use functions with return types....
    public void SpawnObject()
    {
        Spawn();
    }
    public GameObject Spawn()
    {
        GameObject instance = GameObject.Instantiate(prefab);

        if (alignPosition)
            instance.transform.position = transform.position;

        if (alignRotation)
            instance.transform.rotation = transform.rotation;

        return instance;
    }
}