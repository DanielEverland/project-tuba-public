using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps the hierarchy clean by instantiating objects within categories
/// </summary>
public static class HierarchyManager
{
    private static Dictionary<HierarchyCategory, Transform> categoryTransforms = new Dictionary<HierarchyCategory, Transform>();

    public static GameObject CreateGameObject(string name, HierarchyCategory category)
    {
        GameObject gameObject = new GameObject(name);
        Add(gameObject, category);

        return gameObject;
    }
    public static GameObject CreateGameObject(HierarchyCategory category)
    {
        GameObject gameObject = new GameObject();
        Add(gameObject, category);

        return gameObject;
    }
    public static T Instantiate<T>(T prefab, HierarchyCategory category) where T : MonoBehaviour
    {
        T instance = Object.Instantiate(prefab);
        Add(instance.gameObject, category);

        return instance;
    }
    public static GameObject Instantiate<T>(GameObject prefab, HierarchyCategory category)
    {
        GameObject instance = GameObject.Instantiate(prefab);
        Add(instance, category);

        return instance;
    }
    public static void Add(GameObject gameObject, HierarchyCategory category)
    {
        if (category == HierarchyCategory.None)
            return;

        EnsureCategoryExists(category);

        gameObject.transform.SetParent(categoryTransforms[category]);
    }
    private static void EnsureCategoryExists(HierarchyCategory category)
    {
        if(!categoryTransforms.ContainsKey(category))
        {
            GameObject categoryGameObject = new GameObject(category.ToString());
            categoryTransforms.Add(category, categoryGameObject.transform);
        }
    }
}