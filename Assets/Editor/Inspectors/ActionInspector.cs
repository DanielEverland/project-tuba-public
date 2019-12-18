using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityAction))]
public class ActionInspector : Editor
{
    private AbilityAction Target => (AbilityAction)target;
    private ScriptableObjectList<AbilityAction.Component> reorderableList;
    private SerializedProperty descriptionProperty;

    private const float PopupSplit = 0.7f;
    private const float PopupSpacing = 3;
    private const float ElementSpacing = 2;

    private readonly Dictionary<ActionEffectType, int> _extraHeight = new Dictionary<ActionEffectType, int>()
    {
        { ActionEffectType.OverTime, 2 }
    };

    private readonly Dictionary<ActionEffectType, System.Action<Rect, SerializedProperty>> _extraInspector = new Dictionary<ActionEffectType, System.Action<Rect, SerializedProperty>>()
    {
        { ActionEffectType.OverTime, DrawOverTimeInspector },
    };

    private void OnEnable()
    {
        SerializedProperty property = serializedObject.FindProperty("components");

        reorderableList = new ScriptableObjectList<AbilityAction.Component>(property, Target.Components);
        reorderableList.onDrawTypePopup += DrawPopup;
        reorderableList.onDrawElementInspector += DrawElement;
        reorderableList.getElementHeightCallback += GetHeight;

        descriptionProperty = serializedObject.FindProperty("description");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(descriptionProperty);
        reorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawElement(Rect rect, SerializedProperty element, GUIContent label, bool selected, bool focused)
    {
        AbilityAction.Component component = (AbilityAction.Component)element.objectReferenceValue;
        ActionEffectType type = component.EffectType;

        rect.y += ElementSpacing;

        if(_extraInspector.ContainsKey(type))
        {
            _extraInspector[type](rect, element);
            
            rect.y += GetExtraHeight(type);
            rect.height -= GetExtraHeight(type);
        }

        reorderableList.DrawElementInspector(rect, element, label, selected, focused);
    }
    private float GetHeight(SerializedProperty element)
    {
        float height = reorderableList.GetHeight(element);

        AbilityAction.Component component = (AbilityAction.Component)element.objectReferenceValue;
        ActionEffectType type = component.EffectType;

        if (_extraHeight.ContainsKey(type))
        {
            height += GetExtraHeight(type);
        }

        return height;
    }
    private float GetExtraHeight(ActionEffectType type)
    {
        if (_extraHeight.ContainsKey(type))
        {
            return _extraHeight[type] * (EditorGUIUtility.singleLineHeight + ElementSpacing) - ElementSpacing;
        }

        return 0;
    }
    private bool DrawPopup(Rect rect, SerializedProperty property)
    {
        Rect newPopUpRect = new Rect(rect)
        {
            width = rect.width * PopupSplit - PopupSpacing,
        };
        Rect effectTypeRect = new Rect(rect)
        {
            x = newPopUpRect.x + newPopUpRect.width + PopupSpacing,
            width = rect.width * (1 - PopupSplit) - PopupSpacing,
        };

        DrawEffectTypePopup(effectTypeRect, property);

        return reorderableList.DrawPopup(newPopUpRect, property);
    }
    private void DrawEffectTypePopup(Rect rect, SerializedProperty property)
    {
        SerializedObject obj = new SerializedObject(property.objectReferenceValue);
        SerializedProperty effectType = obj.FindProperty("effectType");

        EditorGUI.PropertyField(rect, effectType, GUIContent.none);

        obj.ApplyModifiedProperties();
    }


    private static void DrawOverTimeInspector(Rect rect, SerializedProperty property)
    {
        SerializedObject obj = new SerializedObject(property.objectReferenceValue);
        SerializedProperty durationProperty = obj.FindProperty("overTimeDuration");
        SerializedProperty totalTicksProperty = obj.FindProperty("totalTicks");

        rect.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(rect, durationProperty);

        rect.y += EditorGUIUtility.singleLineHeight + 2;

        EditorGUI.PropertyField(rect, totalTicksProperty);

        obj.ApplyModifiedProperties();
    }
}
