using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PatternMoveToPoint))]
public class PatternMoveToPointDrawer : PropertyDrawer
{
    private const float TopPadding = 2;
    private const float Spacing = 2;
    private const int Elements = 2;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
            return;

        SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        serializedObject.Update();

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += TopPadding;

        SerializedProperty animationCurve = serializedObject.FindProperty("curve");
        if (animationCurve != null)
            EditorGUI.PropertyField(position, animationCurve);

        position.y += EditorGUIUtility.singleLineHeight;
        position.y += Spacing;

        SerializedProperty animationTime = serializedObject.FindProperty("animationTime");
        if (animationTime != null)
            EditorGUI.PropertyField(position, animationTime);

        serializedObject.ApplyModifiedProperties();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * Elements + (Spacing * (Elements - 1)) + TopPadding;
    }
}