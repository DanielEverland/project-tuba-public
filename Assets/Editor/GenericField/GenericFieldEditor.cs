using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

public static class GenericFieldEditor
{
    public static void DrawGenericField<T>(Object target, SerializedProperty property, GenericField<T> field, Rect rect, bool allowNull = false) where T : ScriptableObject
    {
        Rect popupRect = new Rect(rect)
        {
            height = EditorGUIUtility.singleLineHeight,
        };

        DrawPopup(target, field, popupRect, allowNull);
        
        Rect inspectorRect = new Rect(popupRect)
        {
            y = popupRect.y + EditorGUIUtility.singleLineHeight,
        };

        if (EnsureInstanceExists(target, field))
            return;

        SerializedObject objectReference = new SerializedObject(field.Instance);
        float height = DrawInspector(field, objectReference, property, inspectorRect);

        EditorGUILayout.GetControlRect(false, height);
    }
    private static void DrawPopup<T>(Object target, GenericField<T> field, Rect rect, bool allowNull) where T : ScriptableObject
    {
        List<Type> availableTypes = TypeLoader.GetTypes(field.Type);
        List<string> options = availableTypes.Select(x => x.Name).ToList();
        int index = GetIndex(field);

        if (allowNull)
            options.Insert(0, "null");

        if(options.Count == 0)
        {
            EditorGUI.HelpBox(rect, $"No options found for {field.Type}", MessageType.Error);
            return;
        }

        int newIndex = EditorGUI.Popup(rect, index, options.ToArray());

        if(newIndex != index)
        {
            // Since we insert "null" at 0, we have to move the index one value down to match the actual type list
            if (allowNull)
                newIndex--;

            Type newType = availableTypes[newIndex];

            RemoveInstance(field);
            AddInstance(target, field, newType);

            AssetDatabase.SaveAssets();
        }
    }
    /// <returns>Inspector height</returns>
    private static float DrawInspector<T>(GenericField<T> field, SerializedObject objectReference, SerializedProperty property, Rect rect) where T : ScriptableObject
    {
        if (field.Instance == null)
            return 0;

        if(PropertyDrawerLoader.Drawers.ContainsKey(field.Type))
        {
            PropertyDrawer customDrawer = PropertyDrawerLoader.Drawers[field.Type];

            rect.height = customDrawer.GetPropertyHeight(property, GUIContent.none);
            customDrawer.OnGUI(rect, property, GUIContent.none);
        }
        else
        {
            rect.height = GeneralPropertyDrawer.Drawer.GetPropertyHeight(objectReference, property, GUIContent.none);
            GeneralPropertyDrawer.Drawer.OnGUI(rect, objectReference, property, GUIContent.none);
        }

        return rect.height;
    }
    /// <returns>True if unable to create instance</returns>
    private static bool EnsureInstanceExists<T>(Object target, GenericField<T> field) where T : ScriptableObject
    {
        if(field.Instance == null)
        {
            List<Type> availableTypes = TypeLoader.GetTypes(field.Type);

            if (availableTypes.Count == 0)
                return true;

            AddInstance(target, field, availableTypes[0]);
        }

        return false;
    }
    private static void RemoveInstance<T>(GenericField<T> field) where T : ScriptableObject
    {
        Object.DestroyImmediate(field.Instance, true);
        field.Instance = null;
    }
    private static void AddInstance<T>(Object target, GenericField<T> field, Type type) where T : ScriptableObject
    {
        ScriptableObject newInstance = ScriptableObject.CreateInstance(type);
        field.Instance = (T)newInstance;

        AssetDatabase.AddObjectToAsset(newInstance, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newInstance));
    }
    private static int GetIndex<T>(GenericField<T> field) where T : ScriptableObject
    {
        if (field.Instance == null)
            return 0;

        return TypeLoader.GetTypes(field.Type).IndexOf(field.Instance.GetType());
    }
}
