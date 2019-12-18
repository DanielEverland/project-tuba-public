using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Axial))]
public class AxialPropertyDrawer : PropertyDrawer
{
    private const int FieldSegments = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DrawLabel(position, label);
        DrawFields(position, property);
    }
    private void DrawLabel(Rect position, GUIContent label)
    {
        Rect labelRect = new Rect(position);
        labelRect.width = EditorGUIUtility.labelWidth;

        EditorGUI.LabelField(labelRect, label);
    }
    private void DrawFields(Rect position, SerializedProperty property)
    {
        SerializedProperty xProperty = property.FindPropertyRelative("x");
        SerializedProperty yProperty = property.FindPropertyRelative("y");

        Rect fieldRect = new Rect(position);

        // For some fucking reason, Unity's Vector2 field is moved one pixel to the left
        fieldRect.x += EditorGUIUtility.labelWidth - 1;
        fieldRect.width = (position.width - EditorGUIUtility.labelWidth) * (2f / 3f);

        GUIContent[] content = new GUIContent[2]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
        };
        int[] values = new int[2]
        {
            xProperty.intValue,
            yProperty.intValue,
        };

        EditorGUI.MultiIntField(fieldRect, content, values);
    }
}