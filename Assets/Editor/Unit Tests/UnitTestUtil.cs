using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Utility class for unit testing
/// </summary>
public class UnitTestUtil
{
    private const string EntityTemplateSearchString = "t:gameobject Unit Test Entity Template";

    public static Entity InstantiateEntity()
    {
        string guid = AssetDatabase.FindAssets(EntityTemplateSearchString)[0];
        string path = AssetDatabase.GUIDToAssetPath(guid);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject instance = GameObject.Instantiate(prefab);

        return instance.GetComponent<Entity>();
    }
}
