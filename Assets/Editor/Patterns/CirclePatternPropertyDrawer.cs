using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CirclePattern))]
public class CirclePatternPropertyDrawer : PropertyDrawer
{
    private const float TopPadding = 2;
    private const float Spacing = 2;
    private const int Elements = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TopPadding;

        SerializedProperty offsetProperty = serializedObject.FindProperty("offset");
        if (offsetProperty != null)
            EditorGUI.PropertyField(position, offsetProperty);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += Spacing;

        SerializedProperty radiusProperty = serializedObject.FindProperty("radius");
        if (radiusProperty != null)
            EditorGUI.PropertyField(position, radiusProperty);

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