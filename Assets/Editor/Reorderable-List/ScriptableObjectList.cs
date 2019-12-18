using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

/// <summary>
/// Reorderble list for ScriptableObjects
/// </summary>
public class ScriptableObjectList<T> : ReorderableList where T : ScriptableObject
{
    private SerializedObject serializedObject => List.serializedObject;
    private Object target => serializedObject.targetObject;
    private IList targetList;

    public event DrawTypePopupDelegate onDrawTypePopup;
    public event DrawElementDelegate onDrawElementInspector;

    /// <summary>
    /// Delegate for drawing the type popup
    /// </summary>
    /// <param name="element">Element in list</param>
    /// <returns>Whether the popup has changed</returns>
    public delegate bool DrawTypePopupDelegate(Rect rect, SerializedProperty element);

    public ScriptableObjectList(SerializedProperty property, IList targetList) : base(property)
    {
        this.targetList = targetList;

        onAddCallback += AddEntry;
        onRemoveCallback += RemoveEntry;
        drawElementCallback += DrawElement;
        getElementHeightCallback += GetHeight;
    }
    public void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        Rect popupRect = new Rect(rect)
        {
            height = EditorGUIUtility.singleLineHeight,
        };

        if(onDrawTypePopup != null)
        {
            if (onDrawTypePopup.Invoke(popupRect, element))
                return;
        }
        else if (DrawPopup(popupRect, element))
            return;
        
        Rect editorRect = new Rect(rect)
        {
            y = rect.y + EditorGUIUtility.singleLineHeight,
            height = GeneralPropertyDrawer.Drawer.GetPropertyHeight(element, label),
        };

        if(onDrawElementInspector != null)
        {
            onDrawElementInspector.Invoke(editorRect, element, label, selected, focused);
        }
        else
        {
            DrawElementInspector(editorRect, element, label, selected, focused);
        }        
    }
    public void DrawElementInspector(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        GeneralPropertyDrawer.Drawer.OnGUI(rect, element, label);
    }
    /// <returns>Whether the type has changed</returns>
    public bool DrawPopup(Rect rect, SerializedProperty element)
    {
        List<Type> availableTypes = TypeLoader.GetTypes(typeof(T)).ToList();
        string[] options = availableTypes.Select(x => x.Name).ToArray();

        int currentIndex = availableTypes.IndexOf(element.objectReferenceValue.GetType());
        int newIndex = EditorGUI.Popup(rect, currentIndex, options);

        if (newIndex != currentIndex)
        {
            int indexOfElement = targetList.IndexOf(element.objectReferenceValue);
            ChangeType(indexOfElement, availableTypes[newIndex]);
            return true;
        }

        return false;
    }
    public float GetHeight(SerializedProperty element)
    {
        return GeneralPropertyDrawer.Drawer.GetPropertyHeight(element, GUIContent.none) + EditorGUIUtility.singleLineHeight;
    }
    public void ChangeType(int index, Type newType)
    {
        RemoveObject(targetList[index] as Object);

        T newModifier = CreateObject(newType);
        targetList[index] = newModifier;

        AssetDatabase.AddObjectToAsset(newModifier, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newModifier));
        AssetDatabase.SaveAssets();
    }
    public void RemoveEntry(ReorderableList list)
    {
        targetList.RemoveAt(list.Index);

        SerializedProperty obj = list.GetItem(list.Index);
        RemoveObject(obj.objectReferenceValue);
    }
    public void RemoveObject(Object obj)
    {
        Object.DestroyImmediate(obj, true);
    }
    public void AddEntry(ReorderableList list)
    {
        List<Type> availableTypes = TypeLoader.GetTypes(typeof(T));

        AddObject(availableTypes[0]);
    }
    public void AddObject(Type type)
    {
        T newModifier = CreateObject(type);
        targetList.Add(newModifier);
        
        AssetDatabase.AddObjectToAsset(newModifier, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newModifier));
        AssetDatabase.SaveAssets();
    }
    public T CreateObject(Type type)
    {
        T newModifier = (T)ScriptableObject.CreateInstance(type);
        newModifier.name = type.Name;

        return newModifier;
    }
}
