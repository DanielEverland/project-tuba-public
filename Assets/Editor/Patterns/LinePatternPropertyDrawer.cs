using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LinePattern))]
public class LinePatternPropertyDrawer : PropertyDrawer
{
    private const float TopPadding = 2;
    private const float Spacing = 2;
    private const int Elements = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TopPadding;

        SerializedProperty startOffsetProperty = serializedObject.FindProperty("startOffset");
        if (startOffsetProperty != null)
            EditorGUI.PropertyField(position, startOffsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += Spacing;

        SerializedProperty endOffsetProperty = serializedObject.FindProperty("endOffset");
        if (endOffsetProperty != null)
            EditorGUI.PropertyField(position, endOffsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += Spacing;

        SerializedProperty countProperty = serializedObject.FindProperty("count");
        if (countProperty != null)
            EditorGUI.PropertyField(position, countProperty);

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * Elements + (Spacing * (Elements - 1)) + TopPadding;
    }
}