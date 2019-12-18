using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Property drawer used to draw <see cref="PatternRotate"/>
/// </summary>
[CustomPropertyDrawer(typeof(PatternRotate))]
public class PatternRotateDrawer : PropertyDrawer
{
    private const float TopPadding = 2;
    private const float Spacing = 2;
    private const int Elements = 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TopPadding;

        SerializedProperty speed = serializedObject.FindProperty("speed");
        if (speed != null)
            EditorGUI.PropertyField(position, speed);
        
        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * Elements + (Spacing * (Elements - 1)) + TopPadding;
    }
}
